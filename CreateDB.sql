create database GitLogAggregatorDB
go

USE GitLogAggregatorDB;
GO

-- Tạo bảng InternshipDirectories
CREATE TABLE InternshipDirectories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    InternshipWeekFolder NVARCHAR(500),
    DateModified DATETIME DEFAULT GETDATE()
);

-- Cập nhật bảng ConfigFiles với khóa ngoại liên kết tới bảng InternshipDirectories
CREATE TABLE ConfigFiles (
    Id INT PRIMARY KEY IDENTITY,
    ProjectDirectory NVARCHAR(255) NOT NULL,
    InternshipDirectoryId INT,
    Author NVARCHAR(100),
    StartDate DATETIME,
    EndDate DATETIME,
    Weeks INT,
    FirstCommitDate DATETIME,
    FOREIGN KEY (InternshipDirectoryId) REFERENCES InternshipDirectories(Id)
);

-- Thêm dữ liệu ban đầu vào bảng mới
INSERT INTO InternshipDirectories (InternshipWeekFolder)
VALUES ('D:\OneDrive\Desktop\GitAggregator\internship_week');
