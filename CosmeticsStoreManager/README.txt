HƯỚNG DẪN CHẠY NHANH

1) Mở SQL Server Management Studio.
2) Chạy file Sql/database.sql để tạo database CosmeticsStoreDB và dữ liệu mẫu.
   Lưu ý: script này reset lại dữ liệu mẫu nếu bảng đã tồn tại.
3) Mở file CosmeticsStoreManager.sln bằng Visual Studio 2022.
4) Kiểm tra chuỗi kết nối trong Data/DbHelper.cs.
   Mặc định đang dùng: Server=.\SQLEXPRESS;Database=CosmeticsStoreDB;
   Nếu máy bạn là LAPTOP-M8KICGBD\SQLEXPRESS thì sửa lại đúng tên server.
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
- Báo cáo sản phẩm bán chạy
- Báo cáo sản phẩm sắp hết

NHỮNG PHẦN ĐÃ SỬA TRONG BẢN NÀY
- Bỏ thư mục build/local như .vs, bin, obj khỏi source nộp bài
- Thêm .gitignore
- Sửa connection string mặc định dễ chạy hơn
- Thêm validate form sản phẩm và khách hàng
- Xử lý lỗi xóa sản phẩm/khách hàng đã phát sinh đơn hàng
- Sửa tạo đơn hàng để trừ tồn kho an toàn hơn
- Thêm bảng báo cáo sản phẩm bán chạy
- Giữ mật khẩu mẫu 123456 theo yêu cầu bài nhóm

GỢI Ý DEMO
1) Đăng nhập admin / 123456.
2) Vào Quản lý sản phẩm, thêm/sửa/tìm kiếm sản phẩm.
3) Vào Quản lý khách hàng, thêm/sửa/tìm kiếm khách hàng.
4) Vào Quản lý đơn hàng, chọn khách + sản phẩm + số lượng rồi lưu đơn.
5) Quay lại sản phẩm để kiểm tra tồn kho đã giảm.
6) Vào Báo cáo thống kê để xem doanh thu, bán chạy, sắp hết hàng.
