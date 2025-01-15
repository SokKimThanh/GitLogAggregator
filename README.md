![image](https://github.com/user-attachments/assets/c2d2d887-6a10-4e16-a6b5-0039b7c4a4d1)

### Hướng Dẫn Sử Dụng Công Cụ Quản Lý Commit Git

#### 1\. Giới Thiệu

Công cụ này hỗ trợ quản lý và tổng hợp commit từ Git theo từng tuần, giúp người dùng dễ dàng theo dõi tiến độ dự án. Ngoài ra, công cụ cho phép xem danh sách thư mục và xóa dữ liệu khi cần.

#### 2\. Cài Đặt

1. Đảm bảo máy tính đã cài đặt Git.

2. Tải công cụ và mở file chạy ứng dụng.

3. Chuẩn bị dự án Git chứa dữ liệu commit.

#### 3\. Hướng Dẫn Sử Dụng

**Bước 1: Danh Mục Dự Án Git**

1. Khi chương trình khởi động, danh mục các dự án sẽ được hiển thị đầu tiên.

2. Nếu muốn chọn thêm dự án mới, nhấn nút "Thêm Dự Án".

3. Chọn thư mục chứa dự án Git của bạn.

4. Chương trình sẽ kiểm tra xem thư mục có chứa repository Git hợp lệ và có commit nào không:

o **Lỗi 1: Thư mục không chứa repository Git hợp lệ:**

§ Thông báo lỗi: "Thư mục được chọn không chứa repository Git hợp lệ. Vui lòng chọn lại."

o **Lỗi 2: Repository Git không chứa commit nào:**

§ Thông báo lỗi: "Repository Git này không chứa bất kỳ commit nào. Vui lòng chọn một repository khác hoặc tạo commit đầu tiên."

5. Nếu hợp lệ, chương trình sẽ:

o Tạo thư mục `internship_week` nếu chưa có.

o Lưu thư mục dự án vào danh mục các dự án.

o Hiển thị ngay tác giả đầu tiên commit và ngày commit đầu tiên.

**Bước 2: Xem Thông Tin Chi Tiết Dự Án**

1. Chọn dự án từ danh mục các dự án.

2. Chương trình sẽ hiển thị thông tin chi tiết về dự án, bao gồm:

o Tác giả đầu tiên commit.

o Ngày commit đầu tiên.

3. Có hai trường hợp xảy ra:

o **Trường hợp 1: Dự án đã tồn tại danh sách các tuần đã tổng hợp:**

§ Danh sách thư mục tuần và danh sách file commit theo tuần đã tổng hợp trong dự án đó sẽ được hiển thị.

o **Trường hợp 2: Dự án chưa có danh sách tổng hợp:**

§ Chương trình sẽ yêu cầu người dùng bấm nút "Tổng Hợp".

**Bước 3: Tổng Hợp Commit**

1. Nhấn nút "Tổng Hợp" để bắt đầu tổng hợp commit.

2. Quá trình tổng hợp sẽ:

o Tạo thư mục tuần bên trong `internship_week`.

o Lấy dữ liệu commit cho từng ngày trong tuần.

o Lưu dữ liệu vào các file riêng theo từng tuần.

3. Sau khi hoàn tất, danh sách tuần sẽ hiển thị trong "Danh Sách Thư Mục".

4. **Lưu ý:** Trong khi quá trình tổng hợp đang chạy, nút tổng hợp sẽ bị vô hiệu hóa cho đến khi hoàn thành.

**Bước 4: Xem Danh Sách Thư Mục**

1. Danh sách các thư mục trong `internship_week` sẽ được hiển thị ở bảng "Danh Sách Thư Mục".

2. Mỗi thư mục đại diện cho một tuần trong dự án.

3. Khi click vào một thư mục trong "Danh Sách Thư Mục", danh sách file trong thư mục đó sẽ hiển thị ở bảng "Danh Sách File".

**Bước 5: Xóa Dữ Liệu**

1. Nhấn nút "Xóa" để xóa thư mục `internship_week` cùng tất cả dữ liệu bên trong.

2. Sau khi xóa, nút xóa sẽ bị vô hiệu hóa cho đến khi thực hiện tổng hợp lại dữ liệu.

3. Danh sách trong "Danh Sách Thư Mục" và "Danh Sách File" sẽ được làm trống.

#### 4\. Tính Năng Chính

1. **Tổng Hợp Commit:**

o Tự động lấy dữ liệu commit theo ngày và lưu vào thư mục tuần.

2. **Quản Lý Trạng Thái Nút:**

o Nút tổng hợp bị vô hiệu hóa khi đang chạy và bật lại sau khi hoàn thành.

o Nút xóa chỉ xuất hiện sau khi đã tổng hợp thành công.

3. **Hiển Thị Danh Sách Thư Mục:**

o Xem nhanh danh sách thư mục trong `internship_week`.

4. **Xóa Dữ Liệu:**

o Xóa toàn bộ dữ liệu để khởi động lại quá trình quản lý.

5. **Phân Loại Commit:**

o Tự động lọc và hiển thị các commit không hợp lệ để dễ dàng kiểm tra.

6. **Xuất Báo Cáo:**

o Có thể thêm tính năng xuất dữ liệu commit thành file Excel hoặc CSV để báo cáo.

#### 5\. Lưu Ý Quan Trọng

1. Phải chọn thư mục dự án Git trước khi thực hiện bất kỳ thao tác nào.

2. Không thể tổng hợp commit nếu chưa chọn tác giả và ngày bắt đầu.

3. Xóa dữ liệu sẽ không thể khôi phục được, nên kiểm tra kỹ trước khi thực hiện.

4. Nếu dự án Git chứa quá nhiều commit, quá trình tổng hợp có thể mất thời gian. Vui lòng kiên nhẫn chờ đợi.

#### 6\. Hỗ Trợ

Nếu gặp sự cố khi sử dụng công cụ, vui lòng liên hệ với bộ phận hỗ trợ qua email: 22211tt0063@mail.tdc.edu.vn hoặc truy cập trang web chính thức để biết thêm chi tiết.
