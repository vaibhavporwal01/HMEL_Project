USE HMELDatabase;

-- Add admin users table
CREATE TABLE IF NOT EXISTS AdminUsers (
    AdminID INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(50) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Role VARCHAR(20) DEFAULT 'Admin',
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    LastLogin DATETIME NULL
);

-- Insert default admin user
INSERT INTO AdminUsers (Username, Password, FullName, Email, Role) 
VALUES ('admin', 'admin123', 'System Administrator', 'admin@hmel.com', 'Admin')
ON DUPLICATE KEY UPDATE Username = Username;

-- Modify existing column
ALTER TABLE JobApplications MODIFY COLUMN Status VARCHAR(50) DEFAULT 'Submitted';

-- Add new columns only if they don't exist
-- Check and add ReviewedBy column
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'HMELDatabase' 
AND TABLE_NAME = 'JobApplications' 
AND COLUMN_NAME = 'ReviewedBy';

SET @sql = IF(@col_exists = 0, 
    'ALTER TABLE JobApplications ADD COLUMN ReviewedBy INT NULL', 
    'SELECT "ReviewedBy column already exists" as message');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Check and add ReviewedDate column
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'HMELDatabase' 
AND TABLE_NAME = 'JobApplications' 
AND COLUMN_NAME = 'ReviewedDate';

SET @sql = IF(@col_exists = 0, 
    'ALTER TABLE JobApplications ADD COLUMN ReviewedDate DATETIME NULL', 
    'SELECT "ReviewedDate column already exists" as message');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Check and add ReviewComments column
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'HMELDatabase' 
AND TABLE_NAME = 'JobApplications' 
AND COLUMN_NAME = 'ReviewComments';

SET @sql = IF(@col_exists = 0, 
    'ALTER TABLE JobApplications ADD COLUMN ReviewComments TEXT NULL', 
    'SELECT "ReviewComments column already exists" as message');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Check and add Stage column
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'HMELDatabase' 
AND TABLE_NAME = 'JobApplications' 
AND COLUMN_NAME = 'Stage';

SET @sql = IF(@col_exists = 0, 
    'ALTER TABLE JobApplications ADD COLUMN Stage VARCHAR(20) DEFAULT "Application"', 
    'SELECT "Stage column already exists" as message');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Check and add Priority column
SET @col_exists = 0;
SELECT COUNT(*) INTO @col_exists 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = 'HMELDatabase' 
AND TABLE_NAME = 'JobApplications' 
AND COLUMN_NAME = 'Priority';

SET @sql = IF(@col_exists = 0, 
    'ALTER TABLE JobApplications ADD COLUMN Priority VARCHAR(10) DEFAULT "Normal"', 
    'SELECT "Priority column already exists" as message');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
