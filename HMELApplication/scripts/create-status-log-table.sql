USE HMELDatabase;

-- Create status log table
CREATE TABLE IF NOT EXISTS ApplicationStatusLog (
    LogID INT AUTO_INCREMENT PRIMARY KEY,
    ApplicationID INT NOT NULL,
    PreviousStatus VARCHAR(50),
    NewStatus VARCHAR(50) NOT NULL,
    PreviousStage VARCHAR(20),
    NewStage VARCHAR(20) NOT NULL,
    ChangedBy INT NOT NULL,
    ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Comments TEXT,
    FOREIGN KEY (ApplicationID) REFERENCES JobApplications(ApplicationID),
    FOREIGN KEY (ChangedBy) REFERENCES AdminUsers(AdminID)
);

-- Create indexes (will show error if they exist, but that's OK)
CREATE INDEX idx_application_status_log ON ApplicationStatusLog(ApplicationID, ChangeDate);
CREATE INDEX idx_changed_by ON ApplicationStatusLog(ChangedBy);
CREATE INDEX idx_change_date ON ApplicationStatusLog(ChangeDate);

-- If you get "Duplicate key name" errors, that means the indexes already exist - ignore those errors
SELECT 'Setup completed - ignore any duplicate key name errors above' as Notice;
