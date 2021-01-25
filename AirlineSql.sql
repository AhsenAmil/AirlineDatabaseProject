use airline_data;

SET @@time_zone = 'SYSTEM';

CREATE TABLE Passenger(
    passenger_id INTEGER(255) AUTO_INCREMENT,
    first_name VARCHAR(70) NOT NULL DEFAULT 'Unknown',
    last_name VARCHAR(50) NOT NULL DEFAULT 'Unknown',
    email_address NVARCHAR(100) NOT NULL DEFAULT 'Unknown' UNIQUE,
    phone_number INTEGER(255),
    PRIMARY KEY (passenger_id)
);

INSERT INTO Passenger(first_name, last_name, email_address, phone_number) VALUES ('Ava', 'Gardner', 'ava@mail.com', 5552346),
                                                                   ('Alan', 'Alda', 'alan@mail.com', 5553455),
                                                                   ('Errol', 'Flynn', 'errol@mail.com', 5442323),
                                                                   ('Ian', 'Holm', 'ian@mail.com', 1256789),
                                                                   ('John', 'Flynn', 'john@mail.com', 9673645),
                                                                   ('Lizzie', 'Tomson', 'lizzie@mail.com', 9985664),
                                                                   ('Monty', 'Johnson', 'monty@mail.com', 1126877),
                                                                   ('Gilbert', 'Ashton', 'gilbert@mail.com', 98987634),
                                                                   ('Melissa', 'Selmann', 'melissa@mail.com', 0929374),
                                                                   ('Tod', 'White', 'tod@mail.com', 8564732);
CREATE TABLE Airplane(
    model_number VARCHAR(255),
    registration_number INTEGER(255) AUTO_INCREMENT,
    capacity INTEGER(255),
    PRIMARY KEY (registration_number)
);

ALTER TABLE Airplane
ADD COLUMN plane_name VARCHAR(50) NOT NULL DEFAULT 'Unknown';

INSERT INTO Airplane(model_number, capacity, plane_name) VALUES (101, 30, 'Birdy'),
                                                                (202, 75, 'Sebastian'),
                                                                (303, 120, 'Jojo');

CREATE TABLE Seat(
  seat_number_p1 INTEGER(50),
  seat_number_p2 VARCHAR(3),
  airplane_reg_number INTEGER(255),
  FOREIGN KEY (airplane_reg_number) REFERENCES Airplane(registration_number),
  PRIMARY KEY (seat_number_p1, seat_number_p2, airplane_reg_number)
);

DELIMITER $$

CREATE PROCEDURE FillSeats(IN max_seat INT, IN arn INT)
BEGIN
    DECLARE j INT;

    SET j = 0;

    loop_label: LOOP
        IF j = max_seat THEN
            LEAVE loop_label;
        END IF;

        SET j = j +  1;
        INSERT INTO Seat(seat_number_p1, seat_number_p2,  airplane_reg_number) VALUES (j, 'A', arn),
                                                                                      (j, 'B', arn),
                                                                                      (j, 'C', arn),
                                                                                      (j, 'D', arn),
                                                                                      (j, 'E', arn),
                                                                                      (j, 'F', arn);

    end loop;
end $$

CALL FillSeats((SELECT @max_seat := capacity FROM Airplane WHERE registration_number=3),3);

CREATE TABLE Flight(
    flight_number INTEGER(255) AUTO_INCREMENT,
    price INTEGER(255),
    destination_airport VARCHAR(255) DEFAULT 'Unknown',
    departure_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    departure_airport VARCHAR(255) DEFAULT 'Unknown',
    arrival_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    flight_type VARCHAR(255),
    airplane_reg_number INTEGER(255),
    gate_number VARCHAR(10),
    PRIMARY KEY (flight_number),
    FOREIGN KEY (airplane_reg_number) REFERENCES Airplane(registration_number)
);

/*
 MAD -> FCO, HND, LAX
 HND -> AMS, IST, MAD, JFK
 AMS -> HND, IST, FCO
 IST -> LAX, JFK, FCO, AMS, HND, MAD
 LAX -> IST, MAD, JFK
 JFK -> LAX
 FCO -> MAD
 */

INSERT INTO Flight(gate_number, price, destination_airport, departure_date, departure_airport, arrival_date, flight_type, airplane_reg_number)
VALUES ('B4', 500, 'LAX (Los Angeles)', '2021-03-10 12:30:30', 'JFK (New York)', '2021-03-10 15:40:20', 'International',
        (SELECT registration_number FROM Airplane WHERE registration_number = 2));

CREATE TABLE Ticket(
    ticket_number INTEGER(255) AUTO_INCREMENT,
    seat_number_p1 INTEGER(255),
    seat_number_p2 VARCHAR(255),
    airplane_reg_number INTEGER(255),
    passenger_id INTEGER(255),
    flight_number INTEGER(255),
    PRIMARY KEY (ticket_number),
    FOREIGN KEY (passenger_id) REFERENCES Passenger(passenger_id) ON DELETE CASCADE,
    FOREIGN KEY (flight_number) REFERENCES Flight(flight_number),
    FOREIGN KEY fk_seat_number (seat_number_p1, seat_number_p2, airplane_reg_number) REFERENCES Seat(seat_number_p1, seat_number_p2, airplane_reg_number)
);

INSERT INTO Ticket(seat_number_p1, seat_number_p2, passenger_id, flight_number, airplane_reg_number)
VALUES ((SELECT seat_number_p1 FROM Seat WHERE seat_number_p1 = 10 AND seat_number_p2 = 'C' AND Seat.airplane_reg_number = 1),
        (SELECT seat_number_p2 FROM Seat WHERE seat_number_p1 = 10 AND seat_number_p2 = 'C' AND Seat.airplane_reg_number = 1),
        (SELECT passenger_id FROM Passenger WHERE passenger_id = 10),
        (SELECT flight_number FROM Flight WHERE flight_number = 33),
        (SELECT registration_number FROM Airplane WHERE registration_number = 1));

/* JOIN STATEMENTS */

/* passenger names with ticket price higher than 500 */
SELECT first_name, last_name FROM Ticket JOIN Passenger P on Ticket.passenger_id = P.passenger_id
JOIN Flight F on Ticket.flight_number = F.flight_number WHERE price > 500;

/*  number of tickets for flights grouped by destination */
SELECT destination_airport, COUNT(ticket_number) AS 'Number of Tickets'
FROM (Ticket JOIN Flight ON Ticket.flight_number = Flight.flight_number)
GROUP BY destination_airport;
