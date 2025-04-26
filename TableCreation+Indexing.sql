SELECT T.*
FROM Tasks T
JOIN UserTasks UT ON T.TaskID = UT.TaskID
WHERE UT.UserID = @UserID
  AND (@Status IS NULL OR T.Status = @Status)
  AND (@DueDate IS NULL OR T.DueDate = @DueDate)
ORDER BY T.DueDate ASC
OFFSET @PageSize * (@PageNumber - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY;
