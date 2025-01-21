![image](https://github.com/user-attachments/assets/c4e787bd-a8d2-4bb3-a658-0620f2ef3022)
# **Hướng Dẫn Sử Dụng Công Cụ Quản Lý GitLogAggregator**

## **1. Giới Thiệu**

Công cụ **GitLogAggregator** được thiết kế để quản lý và tổng hợp thông tin lịch sử commit từ các dự án Git. Mục tiêu chính của công cụ là giúp người dùng dễ dàng theo dõi tiến độ dự án, quản lý thông tin commit theo từng tuần, và chuẩn bị dữ liệu để tương tác với AI trong tương lai. Công cụ cung cấp các tính năng chính:

- Quản lý danh sách dự án Git.

- Tổng hợp commit theo tuần và lưu vào thư mục tương ứng.

- Xem danh sách thư mục và file commit.

- Xóa dữ liệu khi cần.

- Xuất báo cáo commit dưới dạng Excel.

---

## **2. Cài Đặt**

### **Yêu Cầu Hệ Thống**

- Hệ điều hành: Windows 10 trở lên.

- .NET Framework 4.7.2 hoặc .NET Core 3.1 trở lên.

- Git được cài đặt và cấu hình trên máy tính.

### **Các Bước Cài Đặt**

1\. Tải file cài đặt phần mềm từ đường dẫn được cung cấp.

2\. Chạy file cài đặt và làm theo hướng dẫn trên màn hình.

3\. Sau khi cài đặt hoàn tất, khởi động phần mềm từ shortcut trên màn hình.

---

## **3. Hướng Dẫn Sử Dụng**

### **3.1. Giao Diện Chính**

Khi khởi động phần mềm, bạn sẽ thấy giao diện chính bao gồm các thành phần sau:

- **Menu chức năng**: Chứa các tùy chọn như "Thêm Dự Án", "Quản lý tác giả", "Xuất báo cáo".

- **Danh sách dự án**: Hiển thị các dự án đã được thêm vào phần mềm.

- **Thông tin chi tiết**: Hiển thị thông tin chi tiết về dự án và tác giả được chọn.

---

### **3.2. Thêm Dự Án**

#### **Bước 1: Chọn Thư Mục Git**

1\. Trên giao diện chính, nhấn nút **"Thêm Dự Án"**.

2\. Chọn thư mục chứa repository Git bằng cách nhấn nút **"Chọn Thư Mục"**.

3\. Phần mềm sẽ kiểm tra xem thư mục có phải là repository Git hợp lệ không. Nếu không, thông báo lỗi sẽ hiển thị.

#### **Bước 2: Nhập Thông Tin Dự Án**

1\. Nhập các thông tin sau:

   - **Ngày bắt đầu thực tập**: Chọn ngày từ DatePicker.

   - **Ngày kết thúc thực tập**: Chọn ngày từ DatePicker.

   - **Số tuần thực tập**: Nhập số tuần từ NumericUpDown.

2\. Nhấn nút **"Xác Nhận"** để thêm dự án.

#### **Bước 3: Kiểm Tra Kết Quả**

- Nếu thêm thành công, dự án sẽ xuất hiện trong danh sách dự án.

- Nếu có lỗi, thông báo lỗi sẽ hiển thị trên màn hình.

---

### **3.3. Quản Lý Tác Giả**

#### **Xem Danh Sách Tác Giả**

1\. Trên giao diện chính, nhấn nút **"Quản Lý Tác Giả"**.

2\. Danh sách tác giả sẽ hiển thị, bao gồm các thông tin như:

   - **Tên tác giả**.

   - **Email tác giả**.

   - **Ngày tạo**.

#### **Thêm Tác Giả Mới**

1\. Nhấn nút **"Thêm Tác Giả"**.

2\. Nhập các thông tin:

   - **Tên tác giả**.

   - **Email tác giả**.

3\. Nhấn nút **"Lưu"** để thêm tác giả mới.

#### **Cập Nhật Thông Tin Tác Giả**

1\. Chọn tác giả từ danh sách.

2\. Nhấn nút **"Sửa"** để cập nhật thông tin.

3\. Nhập thông tin mới và nhấn nút **"Lưu"**.

---

### **3.4. Xuất Báo Cáo**

#### **Xuất Báo Cáo Dự Án**

1\. Trên giao diện chính, nhấn nút **"Xuất Báo Cáo"**.

2\. Chọn loại báo cáo:

   - **Báo cáo dự án**: Xuất thông tin chi tiết về dự án.

   - **Báo cáo tác giả**: Xuất thông tin về các tác giả liên quan đến dự án.

3\. Chọn định dạng xuất (PDF, Excel, CSV).

4\. Nhấn nút **"Xuất"** để tạo báo cáo.

---

### **3.5. Quản Lý Mối Quan Hệ Dự Án - Tác Giả**

#### **Thêm Mối Quan Hệ**

1\. Chọn dự án từ danh sách dự án.

2\. Nhấn nút **"Quản Lý Tác Giả"**.

3\. Chọn tác giả từ danh sách tác giả.

4\. Nhấn nút **"Thêm Mối Quan Hệ"** để liên kết tác giả với dự án.

#### **Xóa Mối Quan Hệ**

1\. Chọn dự án từ danh sách dự án.

2\. Chọn tác giả từ danh sách tác giả liên quan.

3\. Nhấn nút **"Xóa Mối Quan Hệ"** để hủy liên kết.

---

## **4. Xử Lý Lỗi**

### **4.1. Lỗi Không Tìm Thấy Repository Git**

- **Nguyên nhân**: Thư mục được chọn không phải là repository Git hợp lệ.

- **Giải pháp**: Kiểm tra lại thư mục và đảm bảo rằng nó chứa thư mục `.git`.

### **4.2. Lỗi Không Thêm Được Dự Án**

- **Nguyên nhân**: Dự án đã tồn tại trong cơ sở dữ liệu.

- **Giải pháp**: Kiểm tra lại danh sách dự án và chọn thư mục khác.

### **4.3. Lỗi Không Thêm Được Tác Giả**

- **Nguyên nhân**: Email tác giả đã tồn tại trong cơ sở dữ liệu.

- **Giải pháp**: Kiểm tra lại danh sách tác giả và sử dụng email khác.

---

## **5. Hỗ Trợ**

Nếu bạn gặp bất kỳ vấn đề nào khi sử dụng phần mềm, vui lòng liên hệ:

- **Email hỗ trợ**: support@yoursoftware.com

- **Số điện thoại**: +84 123 456 789

- **Website**: https://yoursoftware.com

---

## **6. Kết Luận**

Phần mềm được thiết kế để giúp bạn quản lý dự án và tác giả một cách hiệu quả. Hãy làm theo hướng dẫn trên để sử dụng phần mềm một cách tối ưu nhất. Cảm ơn bạn đã sử dụng sản phẩm của chúng tôi!

---

**Lưu ý**: Hướng dẫn này có thể được cập nhật theo phiên bản mới của phần mềm. Vui lòng kiểm tra trang chủ để tải về phiên bản mới nhất.
![image](https://github.com/user-attachments/assets/d90fd672-968c-4a0f-b601-d1d14f78d358)
![image](https://github.com/user-attachments/assets/69f3704d-224e-4d32-8dcf-691a07577584)
# **Giới Thiệu Chi Tiết Về Cơ Sở Dữ Liệu Của Phần Mềm GitLogAggregator**

Cơ sở dữ liệu (CSDL) của phần mềm **GitLogAggregator** được thiết kế để quản lý và lưu trữ thông tin liên quan đến các dự án Git, lịch sử commit, tác giả, và các thông tin khác trong quá trình thực tập. CSDL được xây dựng với mục tiêu hỗ trợ việc tổng hợp, phân tích và quản lý dữ liệu commit một cách hiệu quả, đồng thời chuẩn bị cho việc tích hợp với AI trong tương lai.

Dưới đây là mô tả chi tiết về các bảng và mối quan hệ trong CSDL:

---

## **1. Bảng `InternshipDirectories`**

- **Mục đích**: Lưu trữ thông tin về các thư mục thực tập.

- **Các trường**:

  - `ID`: Khóa chính, tự động tăng.

  - `InternshipWeekFolder`: Đường dẫn thư mục thực tập.

  - `DateModified`: Ngày sửa đổi thư mục.

  - `CreatedAt`: Ngày tạo bản ghi.

  - `UpdatedAt`: Ngày cập nhật bản ghi.

- **Ràng buộc**:

  - Khóa chính: `ID`.

  - Giá trị mặc định cho `CreatedAt`, `UpdatedAt`, và `DateModified` là thời gian hiện tại.

---

## **2. Bảng `ConfigFiles`**

- **Mục đích**: Lưu trữ thông tin cấu hình của các dự án Git.

- **Các trường**:

  - `ConfigID`: Khóa chính, tự động tăng.

  - `ProjectDirectory`: Đường dẫn thư mục dự án.

  - `InternshipDirectoryId`: Khóa ngoại tham chiếu đến bảng `InternshipDirectories`.

  - `InternshipStartDate`: Ngày bắt đầu thực tập.

  - `InternshipEndDate`: Ngày kết thúc thực tập.

  - `Weeks`: Số tuần thực tập.

  - `FirstCommitDate`: Ngày commit đầu tiên.

  - `FirstCommitAuthor`: Tác giả đầu tiên commit.

  - `CreatedAt`: Ngày tạo bản ghi.

  - `UpdatedAt`: Ngày cập nhật bản ghi.

- **Ràng buộc**:

  - Khóa chính: `ConfigID`.

  - Khóa ngoại: `InternshipDirectoryId` tham chiếu đến `InternshipDirectories`.

  - Giá trị mặc định cho `CreatedAt` và `UpdatedAt`.

  - Ràng buộc kiểm tra `Weeks > 0`.

---

## **3. Bảng `Authors`**

- **Mục đích**: Lưu trữ thông tin về các tác giả commit.

- **Các trường**:

  - `AuthorID`: Khóa chính, tự động tăng.

  - `AuthorName`: Tên tác giả.

  - `AuthorEmail`: Email tác giả (duy nhất).

  - `CreatedAt`: Ngày tạo bản ghi.

  - `UpdatedAt`: Ngày cập nhật bản ghi.

- **Ràng buộc**:

  - Khóa chính: `AuthorID`.

  - Giá trị mặc định cho `CreatedAt` và `UpdatedAt`.

---

## **4. Bảng `ConfigAuthors`**

- **Mục đích**: Lưu trữ mối quan hệ nhiều-nhiều giữa các dự án (`ConfigFiles`) và tác giả (`Authors`).

- **Các trường**:

  - `ConfigID`: Khóa ngoại tham chiếu đến `ConfigFiles`.

  - `AuthorID`: Khóa ngoại tham chiếu đến `Authors`.

  - `CreatedAt`: Ngày tạo bản ghi.

  - `UpdatedAt`: Ngày cập nhật bản ghi.

- **Ràng buộc**:

  - Khóa chính phức hợp: (`ConfigID`, `AuthorID`).

  - Khóa ngoại tham chiếu đến `ConfigFiles` và `Authors`.

  - Giá trị mặc định cho `CreatedAt` và `UpdatedAt`.

---

## **5. Bảng `ProjectWeeks`**

- **Mục đích**: Lưu trữ thông tin về các tuần thực tập trong dự án.

- **Các trường**:

  - `ProjectWeekId`: Khóa chính, tự động tăng.

  - `ProjectWeekName`: Tên tuần thực tập.

  - `WeekStartDate`: Ngày bắt đầu tuần.

  - `WeekEndDate`: Ngày kết thúc tuần.

  - `InternshipDirectoryId`: Khóa ngoại tham chiếu đến `InternshipDirectories`.

  - `CreatedAt`: Ngày tạo bản ghi.

  - `UpdatedAt`: Ngày cập nhật bản ghi.

- **Ràng buộc**:

  - Khóa chính: `ProjectWeekId`.

  - Khóa ngoại: `InternshipDirectoryId` tham chiếu đến `InternshipDirectories`.

  - Giá trị mặc định cho `CreatedAt`, `UpdatedAt`, `WeekStartDate`, và `WeekEndDate`.

---

## **6. Bảng `Commits`**

- **Mục đích**: Lưu trữ thông tin về các commit trong dự án.

- **Các trường**:

  - `CommitId`: Khóa chính, tự động tăng.

  - `CommitHash`: Hash của commit (duy nhất).

  - `CommitMessage`: Nội dung commit.

  - `CommitDate`: Ngày commit.

  - `Author`: Tên tác giả commit.

  - `AuthorEmail`: Email tác giả commit.

  - `ProjectWeekId`: Khóa ngoại tham chiếu đến `ProjectWeeks`.

  - `Date`: Ngày của commit.

  - `Period`: Buổi trong ngày (sáng, chiều, tối).

  - `CreatedAt`: Ngày tạo bản ghi.

  - `UpdatedAt`: Ngày cập nhật bản ghi.

- **Ràng buộc**:

  - Khóa chính: `CommitId`.

  - Khóa ngoại: `ProjectWeekId` tham chiếu đến `ProjectWeeks`.

  - Giá trị mặc định cho `CreatedAt` và `UpdatedAt`.

  - Ràng buộc kiểm tra `CommitDate <= GETDATE()`.

  - Ràng buộc duy nhất cho `CommitHash`.

---

## **7. Bảng `CommitPeriods`**

- **Mục đích**: Lưu trữ thông tin về các buổi (sáng, chiều, tối) trong từng ngày của tuần thực tập.

- **Các trường**:

  - `PeriodID`: Khóa chính, tự động tăng.

  - `PeriodName`: Tên buổi (ví dụ: "Buổi sáng 16/01/2025").

  - `PeriodDuration`: Thời gian buổi (ví dụ: "morning").

  - `PeriodStartDate`: Thời gian bắt đầu buổi.

  - `PeriodEndDate`: Thời gian kết thúc buổi.

  - `CreatedAt`: Ngày tạo bản ghi.

  - `UpdatedAt`: Ngày cập nhật bản ghi.

- **Ràng buộc**:

  - Khóa chính: `PeriodID`.

  - Giá trị mặc định cho `CreatedAt` và `UpdatedAt`.

---

## **8. Bảng `CommitGroupMembers`**

- **Mục đích**: Lưu trữ mối quan hệ nhiều-nhiều giữa các buổi (`CommitPeriods`) và commit (`Commits`).

- **Các trường**:

  - `PeriodID`: Khóa ngoại tham chiếu đến `CommitPeriods`.

  - `CommitId`: Khóa ngoại tham chiếu đến `Commits`.

  - `AddedAt`: Ngày thêm commit vào buổi.

- **Ràng buộc**:

  - Khóa chính phức hợp: (`PeriodID`, `CommitId`).

  - Khóa ngoại tham chiếu đến `CommitPeriods` và `Commits`.

  - Giá trị mặc định cho `AddedAt`.

---

## **9. Bảng `ChatbotSummary`**

- **Mục đích**: Lưu trữ thông tin tóm tắt từ chatbot, bao gồm các nhận xét, ghi chú, và kết quả công việc.

- **Các trường**:

  - `ID`: Khóa chính, tự động tăng.

  - `PeriodID`: Khóa ngoại tham chiếu đến `CommitPeriods`.

  - `Attendance`: Thông tin điểm danh.

  - `AssignedTasks`: Nhiệm vụ được giao.

  - `ContentResults`: Kết quả nội dung.

  - `SupervisorComments`: Nhận xét từ người giám sát.

  - `Notes`: Ghi chú.

  - `CreatedAt`: Ngày tạo bản ghi.

  - `UpdatedAt`: Ngày cập nhật bản ghi.

- **Ràng buộc**:

  - Khóa chính: `ID`.

  - Khóa ngoại: `PeriodID` tham chiếu đến `CommitPeriods`.

  - Giá trị mặc định cho `CreatedAt` và `UpdatedAt`.

---

## **10. Mối Quan Hệ Giữa Các Bảng**

- **`ConfigFiles`** và **`Authors`** có mối quan hệ nhiều-nhiều thông qua bảng **`ConfigAuthors`**.

- **`ProjectWeeks`** liên kết với **`InternshipDirectories`** thông qua khóa ngoại `InternshipDirectoryId`.

- **`Commits`** liên kết với **`ProjectWeeks`** thông qua khóa ngoại `ProjectWeekId`.

- **`CommitPeriods`** và **`Commits`** có mối quan hệ nhiều-nhiều thông qua bảng **`CommitGroupMembers`**.

- **`ChatbotSummary`** liên kết với **`CommitPeriods`** thông qua khóa ngoại `PeriodID`.

---

## **11. Tổng Kết**

Cơ sở dữ liệu của **GitLogAggregator** được thiết kế để hỗ trợ quản lý thông tin commit, tác giả, và các tuần thực tập một cách hiệu quả. Các bảng và mối quan hệ được xây dựng để đảm bảo tính nhất quán, toàn vẹn dữ liệu, và dễ dàng mở rộng trong tương lai. CSDL này là nền tảng quan trọng để phần mềm có thể tổng hợp, phân tích và xuất báo cáo dữ liệu commit một cách chính xác và hiệu quả.
