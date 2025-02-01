--select * from ConfigFiles

--select * from Authors

--select * from InternshipDirectories

--select * from Weeks

--select * from CommitPeriods

--select * from Commits

select * from Summary

select * from CommitSummary

SELECT COUNT(*)
FROM CommitSummary
WHERE CommitID = 86
  AND SummaryID = 11;
