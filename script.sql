CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Roles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Uuid` CHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    `Name` VARCHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Roles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Users` (
    `Id` int(11) NOT NULL AUTO_INCREMENT,
    `Uuid` CHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    `FirstName` VARCHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    `LastName` VARCHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`),
    CONSTRAINT `AK_Users_Uuid` UNIQUE (`Uuid`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Accounts` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Uuid` CHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    `UserUuid` CHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    `Username` VARCHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    `Password` VARCHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Accounts` PRIMARY KEY (`Id`),
    CONSTRAINT `AK_Accounts_Uuid` UNIQUE (`Uuid`),
    CONSTRAINT `FK_Account_User` FOREIGN KEY (`UserUuid`) REFERENCES `Users` (`Uuid`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AccountRoles` (
    `AccountsId` int NOT NULL,
    `RolesId` int NOT NULL,
    CONSTRAINT `PK_AccountRoles` PRIMARY KEY (`AccountsId`, `RolesId`),
    CONSTRAINT `FK_AccountRoles_Accounts_AccountsId` FOREIGN KEY (`AccountsId`) REFERENCES `Accounts` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AccountRoles_Roles_RolesId` FOREIGN KEY (`RolesId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Tokens` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Uuid` CHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    `AccountUuid` CHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    `Value` VARCHAR(100) CHARACTER SET utf8mb4 NOT NULL,
    `Type` int(11) NOT NULL,
    `Status` int(11) NOT NULL,
    CONSTRAINT `PK_Tokens` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Token_Account` FOREIGN KEY (`AccountUuid`) REFERENCES `Accounts` (`Uuid`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_AccountRoles_RolesId` ON `AccountRoles` (`RolesId`);

CREATE INDEX `FK_Account_User` ON `Accounts` (`UserUuid`);

CREATE UNIQUE INDEX `IX_Accounts_UserUuid` ON `Accounts` (`UserUuid`);

CREATE UNIQUE INDEX `UQ_Account_Uuid` ON `Accounts` (`Uuid`);

CREATE UNIQUE INDEX `UQ_Role_Uuid` ON `Roles` (`Uuid`);

CREATE INDEX `FK_Token_Account` ON `Tokens` (`AccountUuid`);

CREATE UNIQUE INDEX `UQ_Token_Uuid` ON `Tokens` (`Uuid`);

CREATE UNIQUE INDEX `UQ_Token_Value` ON `Tokens` (`Value`);

CREATE UNIQUE INDEX `UQ_User_Uuid` ON `Users` (`Uuid`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240913161552_Initial', '8.0.8');

COMMIT;

START TRANSACTION;

ALTER TABLE `Roles` ADD CONSTRAINT `AK_Roles_Uuid` UNIQUE (`Uuid`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240913161822_Init2', '8.0.8');

COMMIT;

START TRANSACTION;

DROP TABLE `Tokens`;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240914133053_Remove table tokens', '8.0.8');

COMMIT;

