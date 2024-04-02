CREATE TABLE users
(
    sequence         SERIAL PRIMARY KEY,
    user_id          UUID        NOT NULL,
    name             VARCHAR(255),
    email            VARCHAR(255),
    status_code      VARCHAR(50) NOT NULL,
    created_date_utc TIMESTAMP DEFAULT (now()),
    updated_date_utc TIMESTAMP DEFAULT (now()),
    CONSTRAINT c_user_id UNIQUE (user_id)
);


CREATE TABLE accounts
(
    sequence         SERIAL PRIMARY KEY,
    user_id          UUID        NOT NULL,
    account_id       UUID        NOT NULL,
    account_number   VARCHAR(50),
    status_code      VARCHAR(50) NOT NULL,
    description      VARCHAR(4000),
    created_date_utc TIMESTAMP DEFAULT (now()),
    updated_date_utc TIMESTAMP DEFAULT (now()),
    FOREIGN KEY (user_id) REFERENCES users (user_id)
);

CREATE INDEX ix_account
    ON accounts (user_id, account_id);


CREATE TABLE financial_details
(
    sequence         SERIAL PRIMARY KEY,
    user_id          UUID        NOT NULL,
    salary_monthly   decimal,
    expenses_monthly decimal,
    status_code      VARCHAR(50) NOT NULL,
    created_date_utc TIMESTAMP DEFAULT (now()),
    updated_date_utc TIMESTAMP DEFAULT (now()),
    FOREIGN KEY (user_id) REFERENCES users (user_id)
);