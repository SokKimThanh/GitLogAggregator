create database GitTongHop
go

use GitTongHop
-- ----------------------------
-- InternshipDirectories Table
-- ----------------------------
CREATE TABLE InternshipDirectories (
    DirectoryID INT PRIMARY KEY,
    Path NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- ----------------------------
-- ConfigFiles Table
-- ----------------------------
CREATE TABLE ConfigFiles (
    ConfigID INT PRIMARY KEY,
    DirectoryID INT NOT NULL,
    FilePath NVARCHAR(MAX) NOT NULL,
    FileType NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (DirectoryID) REFERENCES InternshipDirectories(DirectoryID)
);

-- ----------------------------
-- Authors Table
-- ----------------------------
CREATE TABLE Authors (
    AuthorID INT PRIMARY KEY,
    AuthorName NVARCHAR(255) NOT NULL,
    AuthorEmail NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- ----------------------------
-- ProjectWeeks Table
-- ----------------------------
CREATE TABLE ProjectWeeks (
    WeekID INT PRIMARY KEY, 
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    WeekNumber INT NOT NULL,
 );
-- ----------------------------
-- CommitPeriods Table
-- ----------------------------
CREATE TABLE CommitPeriods (
    PeriodID INT PRIMARY KEY,                 -- Khóa chính cho từng record. 
    PeriodName NVARCHAR(50) NOT NULL,            -- Buổi: Sáng, Chiều, Tối.
    PeriodStart DATETIME NOT NULL,            -- Thời gian bắt đầu.
    PeriodEnd DATETIME NOT NULL,              -- Thời gian kết thúc.
    FOREIGN KEY (CommitID) REFERENCES Commits(CommitID)
);


-- ----------------------------
-- ChatbotSummary Table
-- ----------------------------
CREATE TABLE ChatbotSummary (
    SummaryID INT PRIMARY KEY, 
    Summary NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CommitID) REFERENCES Commits(CommitID)
);

-- ----------------------------
-- Commits Table
-- ----------------------------
CREATE TABLE Commits (
    CommitID INT PRIMARY KEY,
    ConfigID INT NOT NULL,
    AuthorID INT,
	WeekID int,
	PeriodID int,
	SummaryID int, 
    CommitMessages NVARCHAR(MAX) NOT NULL,
    CommitDate DATETIME NOT NULL,
    FOREIGN KEY (ConfigID) REFERENCES ConfigFiles(ConfigID),
    FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID),
	FOREIGN KEY (WeekID) REFERENCES ProjectWeeks(WeekID),
	FOREIGN KEY (SummaryID) REFERENCES ChatbotSummary(SummaryID),
	FOREIGN KEY (PeriodID) REFERENCES CommitPeriods(PeriodID),
);