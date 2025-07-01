USE HMELDatabase;

-- Add foreign key constraint (only if it doesn't exist)
SET @fk_exists = 0;
SELECT COUNT(*) INTO @fk_exists 
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
WHERE TABLE_SCHEMA = 'HMELDatabase' 
AND TABLE_NAME = 'JobApplications' 
AND CONSTRAINT_NAME = 'FK_JobApplications_AdminUsers';

SET @sql = IF(@fk_exists = 0, 
    'ALTER TABLE JobApplications ADD CONSTRAINT FK_JobApplications_AdminUsers FOREIGN KEY (ReviewedBy) REFERENCES AdminUsers(AdminID)', 
    'SELECT "Foreign key constraint already exists" as message');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Create indexes (with error handling)
-- Index for status and stage
SET @idx_exists = 0;
SELECT COUNT(*) INTO @idx_exists 
FROM INFORMATION_SCHEMA.STATISTICS 
WHERE TABLE_SCHEMA = 'HMELDatabase' 
AND TABLE_NAME = 'JobApplications' 
AND INDEX_NAME = 'idx_status_stage';

SET @sql = IF(@idx_exists = 0, 
    'CREATE INDEX idx_status_stage ON JobApplications(Status, Stage)', 
    'SELECT "Index idx_status_stage already exists" as message');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Index for reviewed by
SET @idx_exists = 0;
SELECT COUNT(*) INTO @idx_exists 
FROM INFORMATION_SCHEMA.STATISTICS 
WHERE TABLE_SCHEMA = 'HMELDatabase' 
AND TABLE_NAME = 'JobApplications' 
AND INDEX_NAME = 'idx_reviewed_by';

SET @sql = IF(@idx_exists = 0, 
    'CREATE INDEX idx_reviewed_by ON JobApplications(ReviewedBy)', 
    'SELECT "Index idx_reviewed_by already exists" as message');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Index for priority
SET @idx_exists = 0;
SELECT COUNT(*) INTO @idx_exists 
FROM INFORMATION_SCHEMA.STATISTICS 
WHERE TABLE_SCHEMA = 'HMELDatabase' 
AND TABLE_NAME = 'JobApplications' 
AND INDEX_NAME = 'idx_priority';

SET @sql = IF(@idx_exists = 0, 
    'CREATE INDEX idx_priority ON JobApplications(Priority)', 
    'SELECT "Index idx_priority already exists" as message');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Update existing records
UPDATE JobApplications 
SET Stage = 'Application', Priority = 'Normal' 
WHERE Stage IS NULL OR Stage = '' OR Priority IS NULL OR Priority = '';
