import process from 'node:process';
import * as pg from 'pg'
const { Pool } = pg

const host = process.env.DATABASE_URL;

const pool = new Pool({
    connectionString: host
});


export const db = () => {
    query: (text, params) => pool.query(text, params)
}
