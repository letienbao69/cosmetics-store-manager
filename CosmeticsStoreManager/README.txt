HƯỚNG DẪN CHẠY NHANH

1) Mở SQL Server Management Studio.
2) Chạy file Sql/database.sql để tạo database và dữ liệu mẫu.
3) Mở file CosmeticsStoreManager.csproj bằng Visual Studio 2022.
4) Sửa chuỗi kết nối trong Data/DbHelper.cs cho đúng Server SQL của máy.
5) Restore NuGet rồi chạy chương trình.

TÀI KHOẢN MẪU
- admin / 123456
- staff / 123456

CHỨC NĂNG ĐÃ CÓ
- Đăng nhập phân quyền Admin, Staff
- Quản lý sản phẩm
- Quản lý khách hàng
- Quản lý đơn hàng
- Cập nhật tồn kho sau khi bán
- Báo cáo doanh thu theo ngày
- Báo cáo sản phẩm sắp hết

GỢI Ý NÂNG CẤP
- Băm mật khẩu
- Quản lý nhập kho riêng
- In hóa đơn
- Xuất Excel/PDF
- Dashboard biểu đồ

GIAO DIỆN MỚI
- Login 2 cột, hiện đại hơn
- Dashboard có sidebar + thẻ thông tin
- Form CRUD chia card trái/phải
- Bảng dữ liệu được style lại
