CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;

CREATE TABLE `Chats` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `Users` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext NOT NULL,
    `Email` longtext NOT NULL,
    `Password` longtext NOT NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `ChatUser` (
    `ChatsId` int NOT NULL,
    `UsersId` int NOT NULL,
    PRIMARY KEY (`ChatsId`, `UsersId`),
    CONSTRAINT `FK_ChatUser_Chats_ChatsId` FOREIGN KEY (`ChatsId`) REFERENCES `Chats` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ChatUser_Users_UsersId` FOREIGN KEY (`UsersId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Messages` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Text` longtext NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `Image` longblob NOT NULL,
    `UserId` int NOT NULL,
    `ChatId` int NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Messages_Chats_ChatId` FOREIGN KEY (`ChatId`) REFERENCES `Chats` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Messages_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_ChatUser_UsersId` ON `ChatUser` (`UsersId`);

CREATE INDEX `IX_Messages_ChatId` ON `Messages` (`ChatId`);

CREATE INDEX `IX_Messages_UserId` ON `Messages` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240627084716_Initial', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Users` MODIFY `Password` VARCHAR(255) NOT NULL;

ALTER TABLE `Users` MODIFY `Name` VARCHAR(255) NOT NULL;

ALTER TABLE `Users` MODIFY `Email` VARCHAR(255) NOT NULL;

ALTER TABLE `Chats` MODIFY `Name` VARCHAR(255) NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240628143952_mV2.0', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Messages` MODIFY `Text` VARCHAR(2000) NOT NULL;

ALTER TABLE `Messages` MODIFY `Image` MEDIUMBLOB NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240628144942_mV2.1', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE `Messages` MODIFY `Image` MEDIUMBLOB NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240701073241_mV2.2', '8.0.6');

COMMIT;

