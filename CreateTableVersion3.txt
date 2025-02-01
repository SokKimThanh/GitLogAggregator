CREATE DATABASE GitTongHop;
GO

USE GitTongHop;

-- ----------------------------
-- Table: InternshipDirectories
-- Mục đích: Quản lý thông tin thư mục thực tập theo tuần
-- ----------------------------
CREATE TABLE InternshipDirectories (
    InternshipDirectoryId INT PRIMARY KEY IDENTITY(1,1), -- ID tự động tăng, khóa chính
    InternshipWeekFolder NVARCHAR(MAX) NOT NULL,         -- Tên thư mục tuần thực tập (VD: "Week1")
    InternshipStartDate DATETIME NOT NULL,               -- Ngày bắt đầu thực tập
    InternshipEndDate DATETIME NOT NULL,                 -- Ngày kết thúc thực tập
    CreatedAt DATETIME DEFAULT GETDATE(),                -- Thời gian tạo bản ghi (mặc định: hiện tại)
    UpdatedAt DATETIME DEFAULT GETDATE()                 -- Thời gian cập nhật cuối (mặc định: hiện tại)
);

-- ----------------------------
-- Table: ConfigFiles
-- Mục đích: Lưu trữ thông tin cấu hình và liên kết với thư mục thực tập
-- ----------------------------
CREATE TABLE ConfigFiles (
    ConfigID INT PRIMARY KEY IDENTITY(1,1),              -- ID tự động tăng, khóa chính
    ConfigDirectory NVARCHAR(MAX) NOT NULL,              -- Đường dẫn thư mục chứa file cấu hình
    ConfigWeeks int,									 -- Số tuần được cấu hình (VD: "1,2,3")
    FirstCommitDate DATETIME,                            -- Ngày commit đầu tiên (nếu có)
    FirstCommitAuthor NVARCHAR(255),                     -- Tác giả commit đầu tiên (nếu có)
    InternshipDirectoryId INT NOT NULL,                  -- Khóa ngoại tham chiếu đến InternshipDirectories
    CreatedAt DATETIME DEFAULT GETDATE(),                -- Thời gian tạo bản ghi
    UpdatedAt DATETIME DEFAULT GETDATE(),                -- Thời gian cập nhật cuối
    FOREIGN KEY (InternshipDirectoryId) REFERENCES InternshipDirectories(InternshipDirectoryId)
);

-- ----------------------------
-- Table: Authors
-- Mục đích: Quản lý thông tin tác giả thực hiện commit
-- ----------------------------
CREATE TABLE Authors (
    AuthorID INT PRIMARY KEY IDENTITY(1,1),              -- ID tự động tăng, khóa chính
    AuthorName NVARCHAR(255) NOT NULL,                   -- Tên tác giả (bắt buộc)
    AuthorEmail NVARCHAR(255) UNIQUE,                    -- Email tác giả (duy nhất, không trùng lặp)
    CreatedAt DATETIME DEFAULT GETDATE(),                -- Thời gian tạo bản ghi
    UpdatedAt DATETIME DEFAULT GETDATE()                 -- Thời gian cập nhật cuối
);

-- ----------------------------
-- Table: Weeks
-- Mục đích: Định nghĩa các tuần trong dự án
-- ----------------------------
CREATE TABLE Weeks (
    WeekId INT PRIMARY KEY IDENTITY(1,1),              -- ID tự động tăng, khóa chính
    WeekName NVARCHAR(255) NOT NULL,                     -- Tên tuần (VD: "Tuần 1 - Khởi động")
    WeekStartDate DATETIME NOT NULL,                     -- Ngày bắt đầu tuần
    WeekEndDate DATETIME NOT NULL,                       -- Ngày kết thúc tuần
    CreatedAt DATETIME DEFAULT GETDATE(),                -- Thời gian tạo bản ghi
    UpdatedAt DATETIME DEFAULT GETDATE()                 -- Thời gian cập nhật cuối
);

-- ----------------------------
-- Table: CommitPeriods
-- Mục đích: Quản lý các buổi commit (sáng/chiều/tối)
-- ----------------------------
CREATE TABLE CommitPeriods (
    PeriodID INT PRIMARY KEY IDENTITY(1,1),              -- ID tự động tăng, khóa chính
    PeriodName NVARCHAR(50) NOT NULL UNIQUE,             -- Tên buổi (VD: "Sáng", "Chiều" - duy nhất)
    PeriodDuration NVARCHAR(50),                         -- Thời lượng buổi (VD: "2 giờ")
    PeriodStartTime Time NOT NULL,                   -- Thời gian bắt đầu buổi
    PeriodEndTime Time NOT NULL,                     -- Thời gian kết thúc buổi
    CreatedAt DATETIME DEFAULT GETDATE(),                -- Thời gian tạo bản ghi
    UpdatedAt DATETIME DEFAULT GETDATE()                 -- Thời gian cập nhật cuối
);

-- ----------------------------
-- Table: Summary
-- Mục đích: Tổng hợp kết quả và đánh giá từng tuần
-- ----------------------------
CREATE TABLE Summary (
    SummaryID INT PRIMARY KEY IDENTITY(1,1),             -- ID tự động tăng, khóa chính
    Attendance NVARCHAR(MAX),                            -- Điểm danh (VD: "Có mặt đầy đủ")
    AssignedTasks NVARCHAR(MAX),                         -- Nhiệm vụ được giao
    ContentResults NVARCHAR(MAX),                        -- Kết quả nội dung đạt được
    SupervisorComments NVARCHAR(MAX),                    -- Nhận xét từ người giám sát
    Notes NVARCHAR(MAX),                                 -- Ghi chú thêm
    CreatedAt DATETIME DEFAULT GETDATE(),                -- Thời gian tạo bản ghi
    UpdatedAt DATETIME DEFAULT GETDATE()                 -- Thời gian cập nhật cuối
);

-- ----------------------------
-- Table: Commits
-- Mục đích: Lưu trữ thông tin chi tiết về các commit
-- ----------------------------
CREATE TABLE Commits (
    CommitID INT PRIMARY KEY IDENTITY(1,1),              -- ID tự động tăng, khóa chính
    CommitHash NVARCHAR(255) NOT NULL UNIQUE,            -- Hash của commit (duy nhất, không trùng)
    CommitMessages NVARCHAR(MAX) NOT NULL,               -- Thông điệp commit (VD: "Fix bug login")
    CommitDate DATETIME NOT NULL,                        -- Thời gian thực hiện commit
    ConfigID INT NOT NULL,                               -- Khóa ngoại tham chiếu đến ConfigFiles
    AuthorID INT,                                        -- Khóa ngoại tham chiếu đến Authors
    WeekId INT,                                        -- Khóa ngoại tham chiếu đến Weeks
    PeriodID INT,                                        -- Khóa ngoại tham chiếu đến CommitPeriods
    CreatedAt DATETIME DEFAULT GETDATE(),                -- Thời gian tạo bản ghi
    UpdatedAt DATETIME DEFAULT GETDATE(),                -- Thời gian cập nhật cuối
    FOREIGN KEY (ConfigID) REFERENCES ConfigFiles(ConfigID),
    FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID),
    FOREIGN KEY (WeekId) REFERENCES Weeks(WeekId),
    FOREIGN KEY (PeriodID) REFERENCES CommitPeriods(PeriodID)
);

-- ----------------------------
-- Table: CommitSummary (Bảng trung gian)
-- Mục đích: Liên kết nhiều-nhiều giữa Commits và Summary
-- ----------------------------
CREATE TABLE CommitSummary (
    CommitSummaryID INT PRIMARY KEY IDENTITY(1,1),       -- ID tự động tăng, khóa chính
    CommitID INT NOT NULL,                               -- Khóa ngoại tham chiếu đến Commits
    SummaryID INT NOT NULL,                              -- Khóa ngoại tham chiếu đến Summary
    CreatedAt DATETIME DEFAULT GETDATE(),                -- Thời gian tạo bản ghi
    UpdatedAt DATETIME DEFAULT GETDATE(),                -- Thời gian cập nhật cuối
    FOREIGN KEY (CommitID) REFERENCES Commits(CommitID),
    FOREIGN KEY (SummaryID) REFERENCES Summary(SummaryID),
);

 -- Thêm ràng buộc CHECK cho Weeks
ALTER TABLE Weeks ADD CONSTRAINT CHK_WeekDates CHECK (WeekStartDate <= WeekEndDate);

-- Thêm ràng buộc CHECK cho CommitPeriods
ALTER TABLE CommitPeriods ADD CONSTRAINT CHK_PeriodTimes CHECK (PeriodStartTime < PeriodEndTime);