USE [master]
GO
CREATE DATABASE [DictionariesDB]
GO
USE [DictionariesDB]
GO
CREATE TABLE Languages
(
	id int IDENTITY CONSTRAINT PK_Languages_id PRIMARY KEY,
	name nvarchar(40) NOT NULL
)
GO
CREATE TABLE LanguagePairs
(
	id int IDENTITY CONSTRAINT PK_LanguagePairs_id PRIMARY KEY,
	first_language_id int NOT NULL CONSTRAINT FK_LanguagePairs_first_word_id_ref_Languages_id FOREIGN KEY REFERENCES Languages(id),
	second_language_id int NOT NULL CONSTRAINT FK_LanguagePairs_second_word_id_ref_Languages_id FOREIGN KEY REFERENCES Languages(id)
)
GO
CREATE TABLE Words
(
	id int IDENTITY CONSTRAINT PK_Words_id PRIMARY KEY,
	language_id int CONSTRAINT FK_Words_language_id_ref_Languages_id FOREIGN KEY REFERENCES Languages(id),
	name nvarchar(40) NOT NULL
)
GO
CREATE TABLE WordPairs
(
	id int IDENTITY CONSTRAINT PK_WordPairs_id PRIMARY KEY,
	first_word_id int NOT NULL CONSTRAINT FK_WordPairs_first_word_id_ref_Words_id FOREIGN KEY REFERENCES Words(id),
	second_word_id int NOT NULL CONSTRAINT FK_WordPairs_second_word_id_ref_Words_id FOREIGN KEY REFERENCES Words(id)
)
GO