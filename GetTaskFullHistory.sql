-- Status History
SELECT 'StatusChange' AS Type, TH.ChangedAt, U.UserName, TH.OldStatus, TH.NewStatus, NULL AS Comment, NULL AS Notification
FROM TaskHistory TH
JOIN Users U ON TH.ChangedBy = U.UserID
WHERE TH.TaskID = @TaskID

UNION ALL

-- Comments
SELECT 'Comment' AS Type, TC.CommentedAt, U.UserName, NULL, NULL, TC.Comment, NULL
FROM TaskComments TC
JOIN Users U ON TC.CommentedBy = U.UserID
WHERE TC.TaskID = @TaskID

UNION ALL

-- Notifications
SELECT 'Notification' AS Type, N.CreatedAt, U.UserName, NULL, NULL, NULL, N.Message
FROM Notifications N
JOIN Users U ON N.UserID = U.UserID
WHERE N.TaskID = @TaskID
ORDER BY Type, CreatedAt;
