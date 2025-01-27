-- Xóa stored procedure cũ nếu tồn tại
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_GenerateWorkHistoryReport')
    DROP PROCEDURE sp_GenerateWorkHistoryReport
GO

-- Tạo stored procedure mới
CREATE PROCEDURE sp_GenerateWorkHistoryReport
AS
BEGIN
    SELECT
        W.WeekName AS [Tuần],
        DATENAME(WEEKDAY, C.CommitDate) AS [Thứ],
        CP.PeriodName AS [Buổi],
        S.Attendance AS [Điểm danh vắng],
        S.AssignedTasks AS [Công việc được giao],
        S.ContentResults AS [Nội dung - kết quả đạt được],
        S.SupervisorComments AS [Nhận xét - đề nghị của người hướng dẫn tại doanh nghiệp],
        S.Notes AS [Ghi chú]
    FROM
        CommitSummary CS
        INNER JOIN Commits C ON CS.CommitID = C.CommitID
        INNER JOIN Summary S ON CS.SummaryID = S.SummaryID
        INNER JOIN Weeks W ON C.WeekId = W.WeekId
        INNER JOIN CommitPeriods CP ON C.PeriodID = CP.PeriodID
    ORDER BY
        W.WeekStartDate,
        CP.PeriodStartTime
END
GO