-- Khai báo biến
DECLARE @searchAllWeeks BIT;
DECLARE @projectWeekId INT;
DECLARE @searchAllAuthors BIT;
DECLARE @authorId INT;
DECLARE @keyword NVARCHAR(255);

-- Gán giá trị cho biến
SET @searchAllWeeks = 1;         -- 1: true (tìm tất cả tuần), 0: false
SET @projectWeekId = NULL;       -- NULL: không lọc theo tuần cụ thể
SET @searchAllAuthors = 1;       -- 1: true (tìm tất cả tác giả), 0: false
SET @authorId = NULL;            -- NULL: không lọc theo tác giả cụ thể
SET @keyword = '';        -- Từ khóa tìm kiếm
SELECT  
	pw.ProjectWeekName, 
	pw.WeekStartDate,
	pw.WeekEndDate,
    cp.PeriodName AS Period,
	cp.PeriodDuration,
	a.AuthorName AS Author,
    c.CommitMessage,
    a.AuthorEmail   
FROM 
    Commits c,
    ProjectWeeks pw,
    Authors a,
	CommitGroupMembers cgm,
    CommitPeriods cp
WHERE 
    -- Điều kiện kết nối các bảng (thay thế JOIN)
    c.ProjectWeekId = pw.ProjectWeekId
    AND c.Author = a.AuthorName
    AND c.CommitId = cgm.CommitId
	and cp.PeriodID = cgm.PeriodID 
    -- Điều kiện lọc theo tiêu chí tìm kiếm
    AND (
        -- Lọc theo tuần (nếu không chọn "Tất cả")
        (@searchAllWeeks = 1 OR @projectWeekId IS NULL OR c.ProjectWeekId = @projectWeekId)
        -- Lọc theo tác giả (nếu không chọn "Tất cả")
        AND (@searchAllAuthors = 1 OR @authorId IS NULL OR a.AuthorID = @authorId)
        -- Lọc theo từ khóa (nếu có)
        AND (ISNULL(@keyword, '') = '' OR c.CommitMessage LIKE '%' + @keyword + '%')
    );