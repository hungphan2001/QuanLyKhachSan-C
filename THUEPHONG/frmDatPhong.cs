﻿using DevExpress.XtraEditors;
using System;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using BussinessLayer;
using DataLayer;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.Shared;

namespace THUEPHONG
{
    public partial class frmDatPhong : DevExpress.XtraEditors.XtraForm
    {
        public frmDatPhong()
        {
            InitializeComponent();
            DataTable tb = myFunctions.laydulieu("SELECT A.IDPHONG, A.TENPHONG, C.DONGIA, A.IDTANG, B.TENTANG FROM tb_Phong A,tb_Tang B,tb_LoaiPhong C WHERE A.IDTANG=B.IDTANG AND A.STATUS=0 AND A.IDLOAIPHONG =C.IDLOAIPHONG ");
            gcPhong.DataSource = tb;
            gcDatPhong.DataSource = tb.Clone();
        }
        frmMain objMain = (frmMain)Application.OpenForms["frmMain"];

        bool _them;
        KHACHHANG _khachhang;
        SANPHAM _sanpham;
        PHONG _phong;
        int _idPhong=0;
        string _tenPhong;
        string _macty;
        string _madvi;
        int _idDP = 0;
        int _rowDatPhong=0;
        SYS_PARAM _param;
        List<OBJ_DPSP> lstDPSP;
        DATPHONG _datphong;
        DATPHONG_CHITITET _datphongchititet;
        DATPHONG_SANPHAM _datphongsanpham;


        GridHitInfo downHitInfor = null;
        void showHideControl(bool t)
        {
            btnThem.Visible = t;
            btnSua.Visible = t;
            btnXoa.Visible = t;
            btnLuu.Visible = !t;
            btnBoQua.Visible = !t;
            btnIn.Visible = t;
        }

        void _enabled(bool t)
        {
            cbKhachHang.Enabled = t;
            btnAddNew.Enabled = t;
            dtNgayDat.Enabled = t;
            dtNgayTra.Enabled = t;
            cbTrangThai.Enabled = t;
            chkDoan.Checked = true;
            spSoNguoi.Enabled = t;
            txtGhiChu.Enabled = t;
            gcPhong.Enabled = t;
            gcDatPhong.Enabled = t;
            gcSanPham.Enabled = t;
            gcSPDV.Enabled = t;
        }

        void _reset()
        {
            dtNgayDat.Value = DateTime.Now;
            dtNgayTra.Value = DateTime.Now.AddDays(1);
            spSoNguoi.Text = "1";
            chkDoan.Checked = true;
            cbTrangThai.SelectedValue = false;
            txtGhiChu.Text = "";
            txtThanhTien.Text = "0";

        }

        private void frmDatPhong_Load(object sender, EventArgs e)
        {
            _khachhang = new KHACHHANG();
            _sanpham = new SANPHAM();
            _param = new SYS_PARAM();
            _phong = new PHONG();
            _datphong = new DATPHONG();
            _datphongsanpham = new DATPHONG_SANPHAM();
            _datphongchititet = new DATPHONG_CHITITET();

            lstDPSP = new List<OBJ_DPSP>();
            dtTuNgay.Value = myFunctions.GetFirstDayInMont(DateTime.Now.Year, DateTime.Now.Month);
            dtDenNgay.Value = DateTime.Now;
             var _prm = _param.GetParam();
            _macty = _prm.MACTY;
            _madvi = _prm.MADVI;
            loadKH();
            loadSP();
            loadDanhSach();
            cbTrangThai.DataSource = TRANGTHAI.getList();
            cbTrangThai.ValueMember = "_value";
            cbTrangThai.DisplayMember = "_display";
            _enabled(false);
            showHideControl(true);
            gvPhong.ExpandAllGroups();
            tabDanhSach.SelectedTabPage = pageDanhSach;
            txtThanhTien.ReadOnly = true;
            txtThanhTien.BackColor = Color.White;
            txtThanhTien.ForeColor = Color.Red;

        }

        void loadDanhSach()
        {
            _datphong = new DATPHONG();
            //gcDanhSach.DataSource = _datphong.getAll(dtTuNgay.Value, dtDenNgay.Value, _macty, _madvi);
            gcDanhSach.DataSource = _datphong.getAll(dtTuNgay.Value, dtDenNgay.Value);
            gvDanhSach.OptionsBehavior.Editable = false;
        }

        public void loadKH()
        {
            _khachhang = new KHACHHANG();
            cbKhachHang.DataSource = _khachhang.getAll();
            cbKhachHang.DisplayMember = "HOTEN";
            cbKhachHang.ValueMember = "IDKH";
        }

        void loadSP()
        {
            gcSanPham.DataSource = _sanpham.getAll();
            gvSanPham.OptionsBehavior.Editable = false;
        }

        void addReset()
        {
            DataTable tb = myFunctions.laydulieu("SELECT A.IDPHONG, A.TENPHONG, C.DONGIA, A.IDTANG, B.TENTANG FROM tb_Phong A,tb_Tang B,tb_LoaiPhong C WHERE A.IDTANG=B.IDTANG AND A.STATUS=0 AND A.IDLOAIPHONG =C.IDLOAIPHONG ");
            gcPhong.DataSource = tb;
            gcDatPhong.DataSource = tb.Clone();
            gvPhong.ExpandAllGroups();
            gcSPDV.DataSource = _datphongsanpham.getAllByDatPhong(0);
            txtThanhTien.Text = "0";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            _them = true;
            showHideControl(false);
            _enabled(true);
            _reset();
            addReset();
            tabDanhSach.SelectedTabPage = pageChiTiet;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            _them = false;
            _enabled(true);
            showHideControl(false);
            tabDanhSach.SelectedTabPage = pageChiTiet;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn xóa không hử", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _datphong.delete(_idDP);
                var lstDPCT = _datphongchititet.getAllByDatPhong(_idDP);
                foreach (var item in lstDPCT)
                {
                    _phong.updateStatus(item.IDPHONG, false);
                }
            }
            loadDanhSach();
            objMain.gControl.Gallery.Groups.Clear();
            objMain.showRoom();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            saveData();
            objMain.gControl.Gallery.Groups.Clear();
            objMain.showRoom();
            loadDanhSach();
            _enabled(false);
            showHideControl(true);

        }

        void saveData()
        {
            if (_them)
            {
                tb_DatPhong dp = new tb_DatPhong();
                tb_DatPhong_CT dpct;
                tb_DatPhong_SanPham dpsp;
                dp.NGAYDATPHONG = dtNgayDat.Value;
                dp.NGAYTRAPHONG = dtNgayTra.Value;
                dp.SONGUOIO = int.Parse(spSoNguoi.EditValue.ToString());
                dp.STATUS = bool.Parse(cbTrangThai.SelectedValue.ToString());
                dp.THEODOAN = chkDoan.Checked;
                dp.DISABLED = false;
                dp.IDKH=cbKhachHang.SelectedValue.ToString();
                dp.SOTIEN = double.Parse(txtThanhTien.Text);
                dp.GHICHU = txtGhiChu.Text;
                dp.IDUSER = 1;
                dp.MACTY = _macty;
                dp.MADVI = _madvi;
                dp.CREATED_DATE = DateTime.Now;
                var _dp=_datphong.add(dp);
                _idDP = _dp.IDDP;
                for (int i=0; i < gvDatPhong.RowCount; i++)
                {
                    dpct = new tb_DatPhong_CT();
                    dpct.IDDP = _dp.IDDP;
                    dpct.IDPHONG = int.Parse(gvDatPhong.GetRowCellValue(i,"IDPHONG").ToString());
                    dpct.SONGAYO = dtNgayTra.Value.Day - dtNgayDat.Value.Day;
                    dpct.DONGIA = int.Parse(gvDatPhong.GetRowCellValue(i,"DONGIA").ToString());
                    dpct.THANHTIEN = dpct.SONGAYO = dpct.DONGIA;
                    dpct.NGAY = DateTime.Now;
                    var _dpct= _datphongchititet.add(dpct);
                    _phong.updateStatus(dpct.IDPHONG, true);
                    if (gvSPDV.RowCount > 0)
                    {
                        for (int j = 0; j < gvSPDV.RowCount; j++)
                        {
                            if (dpct.IDPHONG== int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString()))
                            {
                                dpsp = new tb_DatPhong_SanPham();
                                dpsp.IDDP = _dp.IDDP;
                                dpsp.IDDPCT = _dpct.IDDPCT;
                                dpsp.IDPHONG = int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString());
                                dpsp.IDSP = int.Parse(gvSPDV.GetRowCellValue(j, "IDSP").ToString());
                                dpsp.SOLUONG = int.Parse(gvSPDV.GetRowCellValue(j, "SOLUONG").ToString());
                                dpsp.DONGIA = int.Parse(gvSPDV.GetRowCellValue(j, "DONGIA").ToString());
                                dpsp.THANHTIEN = dpsp.SOLUONG * dpsp.DONGIA;
                                _datphongsanpham.add(dpsp);
                            }
                        }
                    }
                }
            }
            else
            {
                //update
                tb_DatPhong dp = _datphong.getItem(_idDP);
                tb_DatPhong_CT dpct;
                tb_DatPhong_SanPham dpsp;
                dp.NGAYDATPHONG = dtNgayDat.Value;
                dp.NGAYTRAPHONG = dtNgayTra.Value;
                dp.SONGUOIO = int.Parse(spSoNguoi.EditValue.ToString());
                dp.STATUS = bool.Parse(cbTrangThai.SelectedValue.ToString());
                dp.IDKH = cbKhachHang.SelectedValue.ToString();
                dp.SOTIEN = double.Parse(txtThanhTien.Text);
                dp.GHICHU = txtGhiChu.Text;
                dp.IDUSER = 1;
                dp.UPDATE_BY = 1;
                dp.UPDATE_DATE = DateTime.Now;

                var _dp = _datphong.update(dp);
                _idDP = _dp.IDDP;
                _datphongchititet.deleteAll(_dp.IDDP);
                _datphongsanpham.deleteAll(_dp.IDDP);
                for (int i = 0; i < gvDatPhong.RowCount; i++)
                {
                    dpct = new tb_DatPhong_CT();
                    dpct.IDDP = _dp.IDDP;
                    dpct.IDPHONG = int.Parse(gvDatPhong.GetRowCellValue(i, "IDPHONG").ToString());
                    dpct.SONGAYO = dtNgayTra.Value.Day - dtNgayDat.Value.Day;
                    dpct.DONGIA = int.Parse(gvDatPhong.GetRowCellValue(i, "DONGIA").ToString());
                    dpct.THANHTIEN = dpct.SONGAYO = dpct.DONGIA;
                    dpct.NGAY = DateTime.Now;
                    var _dpct = _datphongchititet.add(dpct);
                    _phong.updateStatus(dpct.IDPHONG, true);
                    if (gvSPDV.RowCount > 0)
                    {
                        for (int j = 0; j < gvSPDV.RowCount; j++)
                        {
                            if (dpct.IDPHONG == int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString()))
                            {
                                dpsp = new tb_DatPhong_SanPham();
                                dpsp.IDDP = _dp.IDDP;
                                dpsp.IDDPCT = _dpct.IDDPCT;
                                dpsp.IDPHONG = int.Parse(gvSPDV.GetRowCellValue(j, "IDPHONG").ToString());
                                dpsp.IDSP = int.Parse(gvSPDV.GetRowCellValue(j, "IDSP").ToString());
                                dpsp.SOLUONG = int.Parse(gvSPDV.GetRowCellValue(j, "SOLUONG").ToString());
                                dpsp.DONGIA = int.Parse(gvSPDV.GetRowCellValue(j, "DONGIA").ToString());
                                dpsp.THANHTIEN = dpsp.SOLUONG * dpsp.DONGIA;
                                _datphongsanpham.add(dpsp);
                            }
                        }
                    }
                }
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            _them = false;
            _enabled(false);
            showHideControl(true);
            tabDanhSach.SelectedTabPage = pageDanhSach;
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            if (!_them)
            {
                saveData();
                XuatReport("PHIEU_DATPHONG", "Phiếu phòng chi tiết");
            }

        }

        private void XuatReport(string _reportName, string _tieude)
        {
            if (_idDP != 0)
            {
                Form frm = new Form();
                CrystalReportViewer Crv = new CrystalReportViewer();
                Crv.ShowGroupTreeButton = false;
                Crv.ShowParameterPanelButton = false;
                Crv.ToolPanelView = ToolPanelViewType.None;
                TableLogOnInfo Thongtin;
                ReportDocument doc = new ReportDocument();
                doc.Load(System.Windows.Forms.Application.StartupPath + "\\Report\\" + _reportName + @".rpt");
                Thongtin = doc.Database.Tables[0].LogOnInfo;
                Thongtin.ConnectionInfo.ServerName = myFunctions._srv;
                Thongtin.ConnectionInfo.DatabaseName = myFunctions._db;
                //Thongtin.ConnectionInfo.UserID = myFunctions._us;
                //Thongtin.ConnectionInfo.Password = myFunctions._pw;
                doc.Database.Tables[0].ApplyLogOnInfo(Thongtin);
                try
                {
                    doc.SetParameterValue("@IDDP", _idDP.ToString());
                    Crv.Dock = DockStyle.Fill;
                    Crv.ReportSource = doc;
                    frm.Controls.Add(Crv);
                    Crv.Refresh();
                    frm.Text = _tieude;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.ToString());
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvDatPhong_MouseDown(object sender, MouseEventArgs e)
        {
            if (gvDatPhong.GetFocusedRowCellValue("IDPHONG") != null)
            {
                _idPhong = int.Parse(gvDatPhong.GetFocusedRowCellValue("IDPHONG").ToString());
                _tenPhong = gvDatPhong.GetFocusedRowCellValue("TENPHONG").ToString();
            }
            GridView view = sender as GridView;
            downHitInfor = null;
            GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None) return;
            if(e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
            {
                downHitInfor = hitInfo;
            }
        }

        private void gvDatPhong_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Button == MouseButtons.Left && downHitInfor != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfor.HitPoint.X - dragSize.Width / 2, downHitInfor.HitPoint.Y - dragSize.Height / 2), dragSize);
                if(!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    DataRow row = view.GetDataRow(downHitInfor.RowHandle);
                    view.GridControl.DoDragDrop(row, DragDropEffects.Move);
                    downHitInfor = null;
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }
        }

        private void gvPhong_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Button == MouseButtons.Left && downHitInfor != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfor.HitPoint.X - dragSize.Width / 2, downHitInfor.HitPoint.Y - dragSize.Height / 2), dragSize);
                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    DataRow row = view.GetDataRow(downHitInfor.RowHandle);
                    view.GridControl.DoDragDrop(row, DragDropEffects.Move);
                    downHitInfor = null;
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }
        }

        private void gvPhong_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            downHitInfor = null;
            GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None) return;
            if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
            {
                downHitInfor = hitInfo;
            }
        }

        private void gcPhong_DragDrop(object sender, DragEventArgs e)
        {
            GridControl grid = sender as GridControl;
            DataTable table = grid.DataSource as DataTable;
            DataRow row = e.Data.GetData(typeof(DataRow)) as DataRow;
            if(row != null && table !=null && row.Table != table)
            {
                table.ImportRow(row);
                row.Delete();
            }
        }

        private void gcPhong_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataRow)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        //STT
        bool cal(Int32 _Width ,GridView _View)
        {
            _View.IndicatorWidth = _View.IndicatorWidth < _Width ? _Width : _View.IndicatorWidth;
            return true;
        }

        private void gvPhong_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if(!gvPhong.IsGroupRow(e.RowHandle))
            {
                if(e.Info.IsRowIndicator)
                {
                    if(e.RowHandle<0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1;
                        e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    }
                    SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                    Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                    BeginInvoke(new MethodInvoker(delegate { cal(_Width, gvPhong); }));
                }    
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));
                SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                BeginInvoke(new MethodInvoker(delegate { cal(_Width, gvPhong); }));
            }
        }

        //Show empty room in id tang
        private void gvPhong_CustomDrawGroupRow(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            string caption = info.Column.Caption;
            if (info.Column.Caption == string.Empty)
                caption = info.Column.ToString();
            info.GroupText = string.Format("{0}:{1} ({2} phòng trống)",caption,info.GroupValueText,view.GetChildRowCount(e.RowHandle));
        }

        private void gcSanPham_DoubleClick(object sender, EventArgs e)
        {
            if (_idPhong == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng?", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (bool.Parse(cbTrangThai.SelectedValue.ToString()) == true)
            {
                MessageBox.Show("Phòng đã hoàn tất không dc chỉnh sửa", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (gvSanPham.GetFocusedRowCellValue("IDSP")!=null)
            {
                OBJ_DPSP sp = new OBJ_DPSP();
                sp.IDSP = int.Parse(gvSanPham.GetFocusedRowCellValue("IDSP").ToString());
                sp.TENSP = gvSanPham.GetFocusedRowCellValue("TENSP").ToString();
                sp.IDPHONG = _idPhong;
                sp.TENPHONG = _tenPhong;
                sp.SOLUONG = 1;
                sp.DONGIA= float.Parse(gvSanPham.GetFocusedRowCellValue("DONGIA").ToString());
                sp.THANHTIEN = sp.DONGIA * sp.SOLUONG;
                foreach (var item in lstDPSP)
                {
                    if (item.IDSP == sp.IDSP && item.IDPHONG==sp.IDPHONG)
                    {
                        item.SOLUONG = item.SOLUONG + 1;
                        item.THANHTIEN = item.SOLUONG * item.DONGIA;
                        loadDPSP();
                        return;
                    }
                    
                }
                lstDPSP.Add(sp);
            }
            loadDPSP();
            txtThanhTien.Text = (double.Parse(gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue.ToString())+ double.Parse(gvDatPhong.Columns["DONGIA"].SummaryItem.SummaryValue.ToString())).ToString("N0");
        }

        void loadDPSP()
        {
            List<OBJ_DPSP> lsDP = new List<OBJ_DPSP>();
            foreach (var item in lstDPSP)
            {
                lsDP.Add(item);
            }
            gcSPDV.DataSource = lstDPSP;
        }

        private void gvSPDV_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if(e.Column.FieldName=="SOLUONG")
            {
                int sl = int.Parse(e.Value.ToString());
                if (sl != 0)
                {
                    double gia=double.Parse(gvSPDV.GetRowCellValue(gvSPDV.FocusedRowHandle,"DONGIA").ToString());
                    gvSPDV.SetRowCellValue(gvSPDV.FocusedRowHandle, "THANHTIEN", sl * gia);
                }
                else
                {
                    gvSPDV.SetRowCellValue(gvSPDV.FocusedRowHandle, "THANHTIEN", 0);
                }
            }
            gvSPDV.UpdateTotalSummary();
            txtThanhTien.Text = (double.Parse(gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue.ToString()) + double.Parse(gvDatPhong.Columns["DONGIA"].SummaryItem.SummaryValue.ToString())).ToString("N0");
        }

        private void gvDatPhong_RowCountChanged(object sender, EventArgs e)
        {
            if (gvDatPhong.RowCount < _rowDatPhong && _them == false)
            {
                _phong.updateStatus(_idPhong, false);
                _datphongchititet.delete(_idDP,_idPhong);
                _datphongsanpham.deleteByDatPhong(_idDP, _idPhong);
                objMain.gControl.Gallery.Groups.Clear();
                objMain.showRoom();
            }

            double t = 0;
            if (gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue == null)
                t = 0;
            else
                t = double.Parse(gvSPDV.Columns["THANHTIEN"].SummaryItem.SummaryValue.ToString());
            txtThanhTien.Text = (double.Parse(gvDatPhong.Columns["DONGIA"].SummaryItem.SummaryValue.ToString()) + t).ToString("N0");
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmKhachHang frm = new frmKhachHang();
            frm.ShowDialog();
        }
        public void setKhachHang(string idkh)
        {
            var _kh = _khachhang.getItem(idkh);
            cbKhachHang.SelectedValue = _kh.IDKH;
            cbKhachHang.Text = _kh.HOTEN;
        }

        private void gvDanhSach_Click(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                _idDP = int.Parse(gvDanhSach.GetFocusedRowCellValue("IDDP").ToString());
                var dp = _datphong.getItem(_idDP);
                cbKhachHang.SelectedValue = dp.IDKH;
                dtNgayDat.Value = dp.NGAYDATPHONG.Value;
                //dtNgayTra.Value = DateTime.Now;
                dtNgayTra.Value = dp.NGAYTRAPHONG.Value;
                spSoNguoi.Text = dp.SONGUOIO.ToString();
                cbTrangThai.SelectedValue = dp.STATUS;
                txtGhiChu.Text = dp.GHICHU;
                txtThanhTien.Text = dp.SOTIEN.Value.ToString();
                loadDP();
                loadSPDV();
            }
        }

        void loadDP()
        {
            _rowDatPhong = 0;
            gcDatPhong.DataSource = myFunctions.laydulieu("SELECT A.IDPHONG, A.TENPHONG, C.DONGIA, A.IDTANG, B.TENTANG FROM tb_Phong A,tb_Tang B,tb_LoaiPhong C ,tb_DatPhong_CT D WHERE A.IDTANG = B.IDTANG AND A.IDLOAIPHONG = C.IDLOAIPHONG AND A.IDPHONG = D.IDPHONG AND D.IDDP = '"+_idDP+"'");
            _rowDatPhong = gvDatPhong.RowCount;
        }

        void loadSPDV()
        {
            gcSPDV.DataSource = _datphongsanpham.getAllByDatPhong(_idDP);
        }

        private void dtTuNgay_ValueChanged(object sender, EventArgs e)
        {
            if (dtTuNgay.Value > dtDenNgay.Value)
            {
                MessageBox.Show("Ngày không hợp lê","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                loadDanhSach();
            }
        }



        private void dtDenNgay_Leave(object sender, EventArgs e)
        {
            if (dtTuNgay.Value > dtDenNgay.Value)
            {
                MessageBox.Show("Ngày không hợp lê", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                loadDanhSach();
            }

        }

        private void dtDenNgay_ValueChanged(object sender, EventArgs e)
        {
            if (dtTuNgay.Value > dtDenNgay.Value)
            {
                MessageBox.Show("Ngày không hợp lê", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                loadDanhSach();
            }
        }

        private void dtTuNgay_Leave(object sender, EventArgs e)
        {
            if (dtTuNgay.Value > dtDenNgay.Value)
            {
                MessageBox.Show("Ngày không hợp lê", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                loadDanhSach();
            }

        }

        private void gvDanhSach_DoubleClick(object sender, EventArgs e)
        {
            if (gvDanhSach.RowCount > 0)
            {
                _idDP = int.Parse(gvDanhSach.GetFocusedRowCellValue("IDDP").ToString());
                var dp = _datphong.getItem(_idDP);
                cbKhachHang.SelectedValue = dp.IDKH;
                dtNgayDat.Value = dp.NGAYDATPHONG.Value;
                dtNgayTra.Value = dp.NGAYTRAPHONG.Value;
                spSoNguoi.Text = dp.SONGUOIO.ToString();
                cbTrangThai.SelectedValue = dp.STATUS;
                txtGhiChu.Text = dp.GHICHU;
                txtThanhTien.Text = dp.SOTIEN.Value.ToString();
                loadDP();
                loadSPDV();
            }
            tabDanhSach.SelectedTabPage = pageChiTiet;
        }

        private void gvDanhSach_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!gvDanhSach.IsGroupRow(e.RowHandle))
            {
                if (e.Info.IsRowIndicator)
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1;
                        e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    }
                    SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                    Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                    BeginInvoke(new MethodInvoker(delegate { cal(_Width, gvDanhSach); }));
                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));
                SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                BeginInvoke(new MethodInvoker(delegate { cal(_Width, gvDanhSach); }));
            }
        }

        private void gvDanhSach_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Name == "DISABLED" && bool.Parse(e.CellValue.ToString()) == true)
            {
                Image img = Properties.Resources._132192_delete_icon;
                e.Graphics.DrawImage(img, e.Bounds.X, e.Bounds.Y);
                e.Handled = true;
            }
        }
    }
}