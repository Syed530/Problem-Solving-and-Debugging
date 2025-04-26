SELECT DISTINCT U.UserID, U.UserName
FROM Users U
WHERE U.UserID IN (
    SELECT ChangedBy FROM TaskHistory WHERE TaskID = @TaskID
    UNION
    SELECT CommentedBy FROM TaskComments WHERE TaskID = @TaskID
    UNION
    SELECT UserID FROM Notifications WHERE TaskID = @TaskID
);
