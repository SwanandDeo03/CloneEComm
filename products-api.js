const express = require('express');
const router = express.Router();
const pool = require('./db');

// GET all products
router.get('/products', async (req, res) => {
    try {
        const [rows] = await pool.query('SELECT * FROM products');
        res.json(rows);
    } catch (error) {
        console.error('Error fetching products:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});

// GET product by ID
router.get('/products/:id', async (req, res) => {
    try {
        const [rows] = await pool.query('SELECT * FROM products WHERE id = ?', [req.params.id]);
        if (rows.length === 0) {
            return res.status(404).json({ error: 'Product not found' });
        }
        res.json(rows[0]);
    } catch (error) {
        console.error('Error fetching product:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});

// POST new product
router.post('/products', async (req, res) => {
    try {
        const { name, price, description, category, image } = req.body;
        
        // Validate required fields
        if (!name || !price || !description || !category) {
            return res.status(400).json({ error: 'Missing required fields' });
        }

        const [result] = await pool.query(
            'INSERT INTO products (name, price, description, category, image) VALUES (?, ?, ?, ?, ?)',
            [name, price, description, category, image]
        );

        res.status(201).json({
            id: result.insertId,
            name,
            price,
            description,
            category,
            image
        });
    } catch (error) {
        console.error('Error creating product:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});

// GET products by category
router.get('/products/category/:category', async (req, res) => {
    try {
        const [rows] = await pool.query('SELECT * FROM products WHERE category = ?', [req.params.category]);
        res.json(rows);
    } catch (error) {
        console.error('Error fetching products by category:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});

// Search products
router.get('/products/search/:query', async (req, res) => {
    try {
        const query = `%${req.params.query}%`;
        const [rows] = await pool.query(
            'SELECT * FROM products WHERE name LIKE ? OR description LIKE ?',
            [query, query]
        );
        res.json(rows);
    } catch (error) {
        console.error('Error searching products:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});

module.exports = router; 