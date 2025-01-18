
![Screenshot 2025-01-17 174355](https://github.com/user-attachments/assets/376f9ec7-21c3-44c9-8689-93a00014c94e)
![image](https://github.com/user-attachments/assets/2ae0acb8-aa50-4eb9-abc4-996474fb97bf)

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

![image](https://github.com/user-attachments/assets/2367ba4c-ba4d-48ef-b71f-0025164ed26b)

### 1\. **Tổng Quan Database**

-   **Tên database:** `GitLogAggregatorDB`

-   **Mục đích:** Database này được thiết kế để lưu trữ thông tin về các commit trong quá trình thực tập, phân loại commit theo buổi/ngày/tuần, và lưu trữ đánh giá từ chatbot AI về các nhóm commit.

* * * * *

### 2\. **Mô Tả Các Bảng**

#### a. **Bảng InternshipDirectories**

-   **Mục đích:** Lưu trữ thông tin về các thư mục thực tập.

-   **Các cột:**

    -   `ID`: Khóa chính, tự động tăng.

    -   `InternshipWeekFolder`: Tên thư mục thực tập.

    -   `DateModified`: Ngày và giờ thư mục được chỉnh sửa.

    -   `CreatedAt`: Thời gian tạo bản ghi.

    -   `UpdatedAt`: Thời gian cập nhật bản ghi.

#### b. **Bảng ConfigFiles**

-   **Mục đích:** Lưu trữ thông tin cấu hình của các dự án thực tập.

-   **Các cột:**

    -   `ID`: Khóa chính, tự động tăng.

    -   `ProjectDirectory`: Đường dẫn thư mục dự án.

    -   `InternshipDirectoryId`: Khóa ngoại liên kết đến `InternshipDirectories`.

    -   `Author`: Tác giả của dự án.

    -   `StartDate`: Ngày bắt đầu thực tập.

    -   `EndDate`: Ngày kết thúc thực tập.

    -   `Weeks`: Số tuần thực tập.

    -   `FirstCommitDate`: Ngày commit đầu tiên.

    -   `CreatedAt`: Thời gian tạo bản ghi.

    -   `UpdatedAt`: Thời gian cập nhật bản ghi.

#### c. **Bảng ProjectWeeks**

-   **Mục đích:** Lưu trữ thông tin về các tuần trong dự án thực tập.

-   **Các cột:**

    -   `ProjectWeekId`: Khóa chính, tự động tăng.

    -   `WeekStartDate`: Ngày bắt đầu tuần.

    -   `WeekEndDate`: Ngày kết thúc tuần.

    -   `InternshipDirectoryId`: Khóa ngoại liên kết đến `InternshipDirectories`.

    -   `CreatedAt`: Thời gian tạo bản ghi.

    -   `UpdatedAt`: Thời gian cập nhật bản ghi.

#### d. **Bảng Commits**

-   **Mục đích:** Lưu trữ thông tin về các commit.

-   **Các cột:**

    -   `CommitId`: Khóa chính, tự động tăng.

    -   `CommitHash`: Mã hash của commit.

    -   `CommitMessage`: Nội dung commit.

    -   `CommitDate`: Ngày và giờ commit.

    -   `Author`: Tác giả của commit.

    -   `ProjectWeekId`: Khóa ngoại liên kết đến `ProjectWeeks`.

    -   `Date`: Ngày commit.

    -   `Period`: Phạm vi thời gian (buổi/ngày/tuần).

    -   `CreatedAt`: Thời gian tạo bản ghi.

    -   `UpdatedAt`: Thời gian cập nhật bản ghi.

#### e. **Bảng CommitGroups**

-   **Mục đích:** Lưu trữ thông tin về các nhóm commit (theo buổi/ngày/tuần).

-   **Các cột:**

    -   `GroupId`: Khóa chính, tự động tăng.

    -   `GroupName`: Tên nhóm commit.

    -   `TimeRange`: Phạm vi thời gian (buổi/ngày/tuần).

    -   `StartDate`: Ngày và giờ bắt đầu nhóm.

    -   `EndDate`: Ngày và giờ kết thúc nhóm.

    -   `CreatedAt`: Thời gian tạo bản ghi.

    -   `UpdatedAt`: Thời gian cập nhật bản ghi.

#### f. **Bảng CommitGroupMembers**

-   **Mục đích:** Liên kết các commit với nhóm commit.

-   **Các cột:**

    -   `GroupId`: Khóa ngoại liên kết đến `CommitGroups`.

    -   `CommitId`: Khóa ngoại liên kết đến `Commits`.

    -   `AddedAt`: Thời gian commit được thêm vào nhóm.

#### g. **Bảng ChatbotSummary**

-   **Mục đích:** Lưu trữ đánh giá từ chatbot AI cho từng nhóm commit.

-   **Các cột:**

    -   `ID`: Khóa chính, tự động tăng.

    -   `GroupId`: Khóa ngoại liên kết đến `CommitGroups`.

    -   `Attendance`: Thông tin điểm danh.

    -   `AssignedTasks`: Nhiệm vụ được giao.

    -   `ContentResults`: Kết quả nội dung.

    -   `SupervisorComments`: Nhận xét từ người hướng dẫn.

    -   `Notes`: Ghi chú.

    -   `CreatedAt`: Thời gian tạo bản ghi.

    -   `UpdatedAt`: Thời gian cập nhật bản ghi.

* * * * *

### 3\. **Mối Quan Hệ Giữa Các Bảng**

-   **InternshipDirectories** ↔ **ConfigFiles**: Một thư mục thực tập (`InternshipDirectories`) có thể có nhiều cấu hình dự án (`ConfigFiles`).

-   **InternshipDirectories** ↔ **ProjectWeeks**: Một thư mục thực tập (`InternshipDirectories`) có thể có nhiều tuần dự án (`ProjectWeeks`).

-   **ProjectWeeks** ↔ **Commits**: Một tuần dự án (`ProjectWeeks`) có thể có nhiều commit (`Commits`).

-   **CommitGroups** ↔ **CommitGroupMembers**: Một nhóm commit (`CommitGroups`) có thể chứa nhiều commit (`CommitGroupMembers`).

-   **CommitGroups** ↔ **ChatbotSummary**: Mỗi nhóm commit (`CommitGroups`) có một đánh giá từ chatbot AI (`ChatbotSummary`).

* * * * *

### 4\. **Ví Dụ Về Dữ Liệu**

#### a. **InternshipDirectories**

| ID | InternshipWeekFolder | DateModified | CreatedAt | UpdatedAt |
| --- | --- | --- | --- | --- |
| 1 | Week1_16_01_2025 | 2025-01-16 12:00:00 | 2025-01-16 12:00:00 | 2025-01-16 12:00:00 |

#### b. **ConfigFiles**

| ID | ProjectDirectory | InternshipDirectoryId | Author | StartDate | EndDate | Weeks | FirstCommitDate | CreatedAt | UpdatedAt |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | E:\Project1 | 1 | Thanh Sok | 2025-01-16 00:00:00 | 2025-01-22 23:59:59 | 1 | 2025-01-16 06:00:00 | 2025-01-16 12:00:00 | 2025-01-16 12:00:00 |

#### c. **ProjectWeeks**

| ProjectWeekId | WeekStartDate | WeekEndDate | InternshipDirectoryId | CreatedAt | UpdatedAt |
| --- | --- | --- | --- | --- | --- |
| 1 | 2025-01-16 00:00:00 | 2025-01-22 23:59:59 | 1 | 2025-01-16 12:00:00 | 2025-01-16 12:00:00 |

#### d. **Commits**

| CommitId | CommitHash | CommitMessage | CommitDate | Author | ProjectWeekId | Date | Period | CreatedAt | UpdatedAt |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 101 | 416a1033a81b... | Cập nhật giao diện người dùng | 2025-01-16 06:00:00 | Thanh Sok | 1 | 2025-01-16 06:00:00 | morning | 2025-01-16 12:00:00 | 2025-01-16 12:00:00 |

#### e. **CommitGroups**

| GroupId | GroupName | TimeRange | StartDate | EndDate | CreatedAt | UpdatedAt |
| --- | --- | --- | --- | --- | --- | --- |
| 1 | Buổi sáng 16/01/2025 | morning | 2025-01-16 06:00:00 | 2025-01-16 12:00:00 | 2025-01-16 12:00:00 | 2025-01-16 12:00:00 |

#### f. **CommitGroupMembers**

| GroupId | CommitId | AddedAt |
| --- | --- | --- |
| 1 | 101 | 2025-01-16 12:00:00 |

#### g. **ChatbotSummary**

| ID | GroupId | Attendance | AssignedTasks | ContentResults | SupervisorComments | Notes | CreatedAt | UpdatedAt |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | 1 | Có mặt đầy đủ | Cập nhật giao diện người dùng | Cải thiện trải nghiệm người dùng | Hoàn thành tốt | Không có ghi chú | 2025-01-16 12:00:00 | 2025-01-16 12:00:00 |
