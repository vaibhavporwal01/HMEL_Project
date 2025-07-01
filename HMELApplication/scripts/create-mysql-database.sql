-- Create Database
CREATE DATABASE IF NOT EXISTS HMELDatabase;
USE HMELDatabase;

-- Create JobApplications Table
CREATE TABLE IF NOT EXISTS JobApplications (
    ApplicationID INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    FatherName VARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender VARCHAR(10) NOT NULL,
    MaritalStatus VARCHAR(20),
    Email VARCHAR(100) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Address TEXT NOT NULL,
    City VARCHAR(50) NOT NULL,
    State VARCHAR(50) NOT NULL,
    PinCode VARCHAR(10) NOT NULL,
    HighestQualification VARCHAR(50) NOT NULL,
    Specialization VARCHAR(100),
    University VARCHAR(100),
    YearOfPassing VARCHAR(10),
    Percentage VARCHAR(10),
    TotalExperience VARCHAR(20),
    CurrentCompany VARCHAR(100),
    CurrentDesignation VARCHAR(100),
    CurrentSalary VARCHAR(20),
    PositionApplied VARCHAR(100) NOT NULL,
    ExpectedSalary VARCHAR(20),
    PhotoPath VARCHAR(500),
    ResumePath VARCHAR(500),
    AdditionalInfo TEXT,
    ApplicationDate DATETIME NOT NULL,
    Status VARCHAR(20) DEFAULT 'Submitted',
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    ModifiedDate DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Create indexes for better performance
CREATE INDEX idx_email ON JobApplications(Email);
CREATE INDEX idx_application_date ON JobApplications(ApplicationDate);
CREATE INDEX idx_status ON JobApplications(Status);

-- Insert sample data for testing
INSERT INTO JobApplications (
    FirstName, LastName, FatherName, DateOfBirth, Gender, MaritalStatus,
    Email, Phone, Address, City, State, PinCode, HighestQualification,
    Specialization, University, YearOfPassing, Percentage, TotalExperience,
    CurrentCompany, CurrentDesignation, CurrentSalary, PositionApplied,
    ExpectedSalary, ApplicationDate, Status
) VALUES 
('John', 'Doe', 'Robert Doe', '1995-05-15', 'Male', 'Single',
 'john.doe@email.com', '+91 9876543210', '123 Main Street, Sector 1', 
 'Bathinda', 'Punjab', '151001', 'Bachelor\'s', 'Chemical Engineering',
 'Punjab Technical University', '2018', '85%', '3 years',
 'ABC Industries', 'Process Engineer', '₹ 4,50,000', 'Process Engineer',
 '₹ 6,00,000', NOW(), 'Submitted'),

('Jane', 'Smith', 'Michael Smith', '1992-08-22', 'Female', 'Married',
 'jane.smith@email.com', '+91 9876543211', '456 Park Avenue, Block B',
 'Chandigarh', 'Punjab', '160001', 'Master\'s', 'Mechanical Engineering',
 'Chandigarh University', '2015', '88%', '5 years',
 'XYZ Corporation', 'Senior Engineer', '₹ 7,00,000', 'Mechanical Engineer',
 '₹ 9,00,000', NOW(), 'Under Review');
