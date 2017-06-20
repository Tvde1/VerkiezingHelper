DROP TABLE IF EXISTS ElectionParty
DROP TABLE IF EXISTS CoalitionParty;
DROP TABLE IF EXISTS Coalition;
DROP TABLE IF EXISTS Party
DROP TABLE IF EXISTS Election;

CREATE TABLE Election(
	ElectionPk int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name varchar(50) NOT NULL,
	Date datetime NULL,
	AmountOfSeats int NULL
)

CREATE TABLE Party(
	PartyPk int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name varchar(50) NOT NULL,
	LeadCandidate varchar(50) NOT NULL
)

CREATE TABLE Coalition(
	CoalitionPk int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ElectionFk int NOT NULL,
	PresidentFk int NOT NULL,
	Name varchar(50) NOT NULL,
	CONSTRAINT FK_Coalition_Election FOREIGN KEY(ElectionFk) REFERENCES Election (ElectionPk),
	CONSTRAINT FK_Coalition_Party FOREIGN KEY(PresidentFk) REFERENCES Party (PartyPk)
)

CREATE TABLE CoalitionParty(
	CoalitionCk int NOT NULL,
	PartyCk int NOT NULL,
    CONSTRAINT PK_CoalitiePartij PRIMARY KEY CLUSTERED 
    (
	    CoalitionCk ASC,
	    PartyCk ASC
    ),
	CONSTRAINT FK_CoalitionParty_Coalition FOREIGN KEY(CoalitionCk) REFERENCES Coalition (CoalitionPk),
	CONSTRAINT FK_CoalitionParty_Party FOREIGN KEY(PartyCk) REFERENCES Party (PartyPk)
)

CREATE TABLE ElectionParty(
	ElectionCk int NOT NULL,
	PartyCk int NOT NULL,
	AmountOfVotes int NOT NULL,
	AmountOfSeats int NOT NULL,
	PercentOfVotes float NOT NULL,
	CONSTRAINT PK_VerkiezingPartij PRIMARY KEY CLUSTERED 
	(
		ElectionCk ASC,
		PartyCk ASC
	),
	CONSTRAINT FK_ElectionParty_Election FOREIGN KEY(ElectionCk) REFERENCES Election (ElectionPk),
	CONSTRAINT FK_ElectionParty_Party FOREIGN KEY(PartyCk) REFERENCES Party (PartyPk)
)