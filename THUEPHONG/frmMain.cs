﻿using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using BussinessLayer;
using DevExpress.XtraNavBar;
using DevExpress.Utils.Drawing;
using DevExpress.XtraBars.Ribbon;
using DataLayer;
using System.Collections.Generic;
using DevExpress.XtraBars.Ribbon.ViewInfo;

namespace THUEPHONG
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(tb_SYS_USER user)
        {
            InitializeComponent();
        }

        tb_SYS_USER _user;
        TANG _tang;
        PHONG _phong;
        FUNC _func;
        GalleryItem  item = null;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _tang = new TANG();
            _phong = new PHONG();
             _func = new FUNC();
            leftMenu();
            showRoom();
        }
        void leftMenu()
        {
            int i = 0;
            var _lsParent = _func.getParent();
            foreach(var _pr in _lsParent)
            {
                NavBarGroup navBar = new NavBarGroup(_pr.DESCRIPTION);
                navBar.Tag = _pr.FUNC_CODE;
                navBar.Name = _pr.FUNC_CODE;
                navBar.ImageOptions.LargeImageIndex = i;
                i++;
                navBarMain.Groups.Add(navBar);

                var _lsChild = _func.getChild(_pr.FUNC_CODE);
                foreach(var _ch in _lsChild)
                {
                    NavBarItem navItem = new NavBarItem(_ch.DESCRIPTION);
                    navItem.Tag = _ch.FUNC_CODE;
                    navItem.Name = _ch.FUNC_CODE;
                    navItem.ImageOptions.SmallImageIndex = 0;
                    navBar.ItemLinks.Add(navItem);
                }
                navBarMain.Groups[navBar.Name].Expanded = true;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void showRoom()
        {
            _tang = new TANG();
            _phong = new PHONG();
            var lsTang = _tang.getAll();
            gControl.Gallery.ItemImageLayout = ImageLayoutMode.ZoomInside;
            gControl.Gallery.ImageSize = new Size(64, 64);
            gControl.Gallery.ShowItemText = true;
            gControl.Gallery.ShowGroupCaption = true;
            foreach(var t in lsTang)
            {
                var galleryItem = new GalleryItemGroup();
                galleryItem.Caption = t.TENTANG;
                galleryItem.CaptionAlignment = GalleryItemGroupCaptionAlignment.Stretch;
                List <tb_Phong> lsPhong = _phong.getByTang(t.IDTANG);
                foreach (var p in lsPhong)
                {
                    var gc_item = new GalleryItem();
                    gc_item.Caption = p.TENPHONG;
                    gc_item.Value = p.IDPHONG;
                    if (p.STATUS == true)
                        gc_item.ImageOptions.Image = imageList3.Images[1];
                    else
                        gc_item.ImageOptions.Image = imageList3.Images[0];
                    galleryItem.Items.Add(gc_item);
                }
                gControl.Gallery.Groups.Add(galleryItem);
            }
            
        }

        private void navBarMain_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            string func_code = e.Link.Item.Tag.ToString();
            switch (func_code)
            {
                case "CONGTY":
                    {
                        frmCongTy frm = new frmCongTy();
                        frm.ShowDialog();
                        break;
                    }
                case "DONVI":
                    {
                         frmDonVi frm = new frmDonVi();
                        frm.ShowDialog();
                        break;
                    }
                case "KHACHHANG":
                    {
                        frmKhachHang frm = new frmKhachHang();
                        frm.ShowDialog();
                        break;
                    }
                case "TANG":
                    {
                        frmTang frm = new frmTang();
                        frm.ShowDialog();
                        break;
                    }
                case "LOAIPHONG":
                    {
                        frmLoaiPhong frm = new frmLoaiPhong();
                        frm.ShowDialog();
                        break;
                    }
                case "PHONG":
                    {
                        frmPhong frm = new frmPhong();
                        frm.ShowDialog();
                        break;
                    }
                case "SANPHAM":
                    {
                        frmSanPham frm = new frmSanPham();
                        frm.ShowDialog();
                        break;
                    }
                case "THIETBI":
                    {
                        frmThietBi frm = new frmThietBi();
                        frm.ShowDialog();
                        break;
                    }
                case "PHONG_TB":
                    {
                        frmPThietBi frm = new frmPThietBi();
                        frm.ShowDialog();
                        break;
                    }
                case "DATPHONG":
                    {
                        frmDatPhong frm = new frmDatPhong();
                        frm.ShowDialog();
                        break;
                    }
            }
        }

        private void popupMenu1_Popup(object sender, EventArgs e)
        {
            Point point = gControl.PointToClient(Control.MousePosition);
            RibbonHitInfo hitInfo = gControl.CalcHitInfo(point);
            if (hitInfo.InGalleryItem || hitInfo.HitTest == RibbonHitTest.GalleryImage)
                item = hitInfo.GalleryItem;
        }

        private void btnDatPhong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_phong.checkEmpty(int.Parse(item.Value.ToString())))
            {
                MessageBox.Show("Phòng đã được đặt .Vui lòng chọn phòng khác","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            frmDatPhongDon frm = new frmDatPhongDon();
            frm._idPhong = int.Parse(item.Value.ToString());
            frm._them = true;
            frm.ShowDialog();
        }

        private void btnSPDV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!_phong.checkEmpty(int.Parse(item.Value.ToString())))
            {
                MessageBox.Show("Phòng chưa được đặt .Vui lòng chọn phòng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            frmDatPhongDon frm = new frmDatPhongDon();
            frm._idPhong = int.Parse(item.Value.ToString());
            frm._them = false;
            frm.ShowDialog();
        }

        private void btnThanhToan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!_phong.checkEmpty(int.Parse(item.Value.ToString())))
            {
                MessageBox.Show("Phòng chưa được đặt nên không thể thanh toán", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            frmDatPhongDon frm = new frmDatPhongDon();
            frm._idPhong = int.Parse(item.Value.ToString());
            frm._them = false;
            frm.ShowDialog();
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            frmBaoCao frmbc = new frmBaoCao(_user);
            frmbc.ShowDialog();
        }
    }
}
