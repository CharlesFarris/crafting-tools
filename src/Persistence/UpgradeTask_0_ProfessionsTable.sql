-- UpgradeTask_0_ProfessionsTable.sql

CREATE table games
(
	id   INT         NOT NULL AUTO_INCREMENT,
	name VARCHAR(48) NOT NULL,
	PRIMARY KEY (id)
);

INSERT INTO games
	(name)
VALUES ('Final Fantasy XIV')

CREATE TABLE professions
(
	id      INT         NOT NULL AUTO_INCREMNT,
	name    VARCHAR(32) NOT NULL,
	game_id INT NOT_NULL,
	PRIMARY KEY (id),
	CONSTRAINT FOREIGN KEY (game_id) REFERENCES games (id) ON DELETE RESTRICT
);

INSERT INTO professions
	(name, game_id)
VALUES ('Carpenter', 1),
	   ('Blacksmith', 1),
	   ('Armorer', 1),
	   ('Goldsmith', 1),
	   ('Leatherworker', 1),
	   ('Weaver', 1),
	   ('Alchemist', 1),
	   ('Culinarian', 1)
