using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VnBookLibrary.Model.Entities;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;
using System.Globalization;

namespace VnBookLibrary.Web.Areas.Manage.Models
{
    public class PrintBillModel
    {
        public PrintBillModel(Bill bill)
        {
            Bill = bill;           
            PrintDocument = new PrintDocument();
            printPrvDlg = new PrintPreviewDialog();
        }
        public Bill Bill { get; set; }      
        private PrintDocument PrintDocument { get; set; }
        PrintPreviewDialog printPrvDlg { get; set; }        
        public void PrintBill()
        {            
            PrintDocument.PrintPage += new PrintPageEventHandler(CreateBillPDF);            
            printPrvDlg.Document = PrintDocument;            
            printPrvDlg.PrintPreviewControl.Zoom = 1.2;
             ((Form)printPrvDlg).WindowState = FormWindowState.Maximized;
            printPrvDlg.ShowDialog();            
        }
        private void CreateBillPDF(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;
            Font font = new Font("Courier New", 10);
            float FontHeight = font.GetHeight();
            int startX = 10;
            int startY = 10;
            int offset = 40;
            graphic.DrawString("HÓA ĐƠN MUA SÁCH", new Font("Courier New", 16, FontStyle.Bold), new SolidBrush(Color.Green), startX + 250, startY);
            graphic.DrawString("Tên khách hàng:           " + Bill.CustomerName, new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)FontHeight + 20;
            graphic.DrawString("Số điện thoại khách hàng: " + Bill.CustomerPhone, new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)FontHeight + 15;
            if (Bill.CustomerEmail != null)
            {
                graphic.DrawString("Email khách hàng:         " + Bill.CustomerEmail, new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + (int)FontHeight + 20;
            }
            graphic.DrawString("Ngày mua:                 " + Bill.BuyDate, new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)FontHeight + 20;
            graphic.DrawString("Địa chỉ khách hàng:       " + Bill.Address, new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)FontHeight + 20;

            graphic.DrawString("Số hóa đơn:               " + Bill.BillId, new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)FontHeight + 20;

            string top = "Tên sách".PadRight(30) + "Số lượng".PadRight(15) + "Đơn giá".PadRight(20) + "Thành tiền";
            graphic.DrawString(top, new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)FontHeight;
            graphic.DrawString("---------------------------------------------------------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)FontHeight + 20;
            List<BillDetail> list = Bill.BillDetails.ToList();
            foreach (BillDetail billDetail in list)
            {
                Product p = billDetail.Product;

                if (nummerWord(p.ProductName) < 5)
                {
                    graphic.DrawString(p.ProductName, new Font("Courier New", 12), new SolidBrush(Color.Black), startX, startY + offset);
                    graphic.DrawString(billDetail.Quantity.ToString(), new Font("Courier New", 12), new SolidBrush(Color.Black), startX + 320, startY + offset);
                    graphic.DrawString(Convert.ToInt32(p.Price).ToString("c", new CultureInfo("vi-VN")), new Font("Courier New", 12), new SolidBrush(Color.Black), startX + 450, startY + offset);
                    graphic.DrawString(Convert.ToInt32((p.Price * billDetail.Quantity ?? 0)).ToString("c", new CultureInfo("vi-VN")), new Font("Courier New", 12), new SolidBrush(Color.Black), startX + 650, startY + offset);
                    offset = offset + (int)FontHeight + 30;
                }
                else
                {
                    graphic.DrawString(GetWord(0, 4, p.ProductName), new Font("Courier New", 12), new SolidBrush(Color.Black), startX, startY + offset);
                    graphic.DrawString(billDetail.Quantity.ToString(), new Font("Courier New", 12), new SolidBrush(Color.Black), startX + 320, startY + offset);
                    graphic.DrawString(Convert.ToInt32(p.Price).ToString("c", new CultureInfo("vi-VN")), new Font("Courier New", 12), new SolidBrush(Color.Black), startX + 450, startY + offset);
                    graphic.DrawString(Convert.ToInt32((p.Price * billDetail.Quantity ?? 0)).ToString("c", new CultureInfo("vi-VN")), new Font("Courier New", 12), new SolidBrush(Color.Black), startX + 650, startY + offset);
                    offset = offset + (int)FontHeight + 10;
                    graphic.DrawString(GetWord(5, nummerWord(p.ProductName), p.ProductName), new Font("Courier New", 12), new SolidBrush(Color.Black), startX, startY + offset);
                    offset = offset + (int)FontHeight + 30;
                }
            }
            graphic.DrawString("--------------------------------------------------------------------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;

            offset = offset + (int)FontHeight + 10;
            graphic.DrawString("Tổng tiền: ", new Font("Courier New", 14, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            graphic.DrawString(Convert.ToInt32((Bill.IntoMoney ?? 0)).ToString("c", new CultureInfo("vi-VN")), new Font("Courier New", 14, FontStyle.Bold), new SolidBrush(Color.Red), startX + 150, startY + offset);

            offset = offset + (int)FontHeight + 30;
            graphic.DrawString("VNBOOK CẢM ƠN BẠN ĐÃ MUA SÁCH!", new Font("Courier New", 14, FontStyle.Bold), new SolidBrush(Color.Black), startX + 250, startY + offset);
        }
        private int nummerWord(string str)
        {
            var list = str.Trim().Split(' ').ToList().Where(x => x.Trim() != "").ToList();
            return list.Count;
        }
        private string GetWord(int start, int end, string str)
        {
            var list = str.Trim().Split(' ').ToList().Where(x => x.Trim() != "").ToList();
            string rs = "";
            if (start < list.Count)
            {
                if (end - start + 1 <= list.Count)
                {
                    var temp = list.Skip(start).Take(end - start + 1).ToArray();
                    return string.Join(" ", temp);
                }
                else
                {
                    var temp = list.Skip(start).Take(list.Count - start + 1).ToArray();
                    return string.Join(" ", temp);
                }
            }
            return rs;
        }
    }
}