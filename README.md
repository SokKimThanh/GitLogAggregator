![image](https://github.com/user-attachments/assets/27dd9fb2-5a23-4fb5-8d4d-05acdec006b9)

# Hướng Dẫn Sử Dụng Công Cụ Quản Lý Commit Git

## 1. Giới Thiệu

Công cụ này hỗ trợ quản lý và tổng hợp commit từ Git theo từng tuần, giúp người dùng dễ dàng theo dõi tiến độ dự án. Ngoài ra, công cụ cho phép xem danh sách thư mục và xóa dữ liệu khi cần.

## 2. Cài Đặt

1. Đảm bảo máy tính đã cài đặt Git.
2. Tải công cụ và mở file chạy ứng dụng.
3. Chuẩn bị dự án Git chứa dữ liệu commit.

## 3. Hướng Dẫn Sử Dụng

### Bước 1: Chọn Thư Mục Dự Án Git

- Nhấn nút "Chọn Dự Án".
- Chọn thư mục chứa dự án Git của bạn.
- Chương trình sẽ kiểm tra xem thư mục có chứa repository Git hợp lệ và có commit nào không:
  - **Lỗi 1: Thư mục không chứa repository Git hợp lệ**: Nếu không chứa repository Git, chương trình sẽ hiển thị thông báo lỗi "Thư mục được chọn không chứa repository Git hợp lệ. Vui lòng chọn lại."
  - **Lỗi 2: Repository Git không chứa commit nào**: Nếu repository Git không có commit nào, chương trình sẽ hiển thị thông báo lỗi "Repository Git này không chứa bất kỳ commit nào. Vui lòng chọn một repository khác hoặc tạo commit đầu tiên."
  - Nếu hợp lệ, chương trình sẽ:
    - Tạo thư mục `internship_week` nếu chưa có.
    - Hiển thị đường dẫn dự án.
    - Bật các nút chọn tác giả, ngày bắt đầu và nút tổng hợp commit.

### Bước 2: Chọn Tác Giả và Ngày Bắt Đầu

- Chọn tác giả từ danh sách "Tác Giả".
- Chọn ngày bắt đầu thực tập từ "Ngày Bắt Đầu".

### Bước 3: Tổng Hợp Commit

- Nhấn nút "Tổng Hợp" để bắt đầu tổng hợp commit.
- Quá trình tổng hợp sẽ:
  1. Tạo thư mục tuần bên trong `internship_week`.
  2. Lấy dữ liệu commit cho từng ngày trong tuần.
  3. Lưu dữ liệu vào các file riêng theo từng tuần.
- Sau khi hoàn tất, danh sách tuần sẽ hiển thị trong "Danh Sách Thư Mục".
- **Lưu ý**: Trong khi quá trình tổng hợp đang chạy, nút tổng hợp sẽ bị vô hiệu hóa cho đến khi hoàn thành.

### Bước 4: Xem Danh Sách Thư Mục

- Danh sách các thư mục trong `internship_week` sẽ được hiển thị ở bảng "Danh Sách Thư Mục".
- Mỗi thư mục đại diện cho một tuần trong dự án.
- Khi click vào một thư mục trong "Danh Sách Thư Mục", danh sách file trong thư mục đó sẽ hiển thị ở bảng "Danh Sách File".

### Bước 5: Xóa Dữ Liệu

- Nhấn nút "Xóa" để xóa thư mục `internship_week` cùng tất cả dữ liệu bên trong.
- Sau khi xóa, nút xóa sẽ bị vô hiệu hóa cho đến khi thực hiện tổng hợp lại dữ liệu.
- Danh sách trong "Danh Sách Thư Mục" và "Danh Sách File" sẽ được làm trống.

## 4. Tính Năng Chính

1. **Tổng Hợp Commit**: Tự động lấy dữ liệu commit theo ngày và lưu vào thư mục tuần.
2. **Quản Lý Trạng Thái Nút**:
   - Nút tổng hợp bị vô hiệu hóa khi đang chạy và bật lại sau khi hoàn thành.
   - Nút xóa chỉ xuất hiện sau khi đã tổng hợp thành công.
3. **Hiển Thị Danh Sách Thư Mục**: Xem nhanh danh sách thư mục trong `internship_week`.
4. **Xóa Dữ Liệu**: Xóa toàn bộ dữ liệu để khởi động lại quá trình quản lý.

## 5. Lưu Ý Quan Trọng

- Phải chọn thư mục dự án Git trước khi thực hiện bất kỳ thao tác nào.
- Không thể tổng hợp commit nếu chưa chọn tác giả và ngày bắt đầu.
- Xóa dữ liệu sẽ không thể khôi phục được, nên kiểm tra kỹ trước khi thực hiện.

## 6. Hỗ Trợ

Nếu gặp sự cố khi sử dụng công cụ, vui lòng liên hệ với bộ phận hỗ trợ qua email: 22211tt0063@mail.tdc.edu.vn hoặc truy cập trang web chính thức để biết thêm chi tiết.
