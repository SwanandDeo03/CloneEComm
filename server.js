const express = require('express');
const cors = require('cors');
const bodyParser = require('body-parser');
const http = require('http');
const https = require('https');
const fs = require('fs');
const path = require('path');
const productsRouter = require('./products-api');

const app = express();
const port = 5501;

// SSL Configuration
const sslOptions = {
    key: fs.readFileSync(path.join(__dirname, 'ssl', 'private.key')),
    cert: fs.readFileSync(path.join(__dirname, 'ssl', 'certificate.crt'))
};

// Middleware
app.use(cors());
app.use(bodyParser.json());
app.use(express.static('.')); // Serve static files from the current directory

// Use the products router
app.use('/api', productsRouter);

// Sample data
const products = [
    {
        id: 1,
        name: "Wireless Headphones",
        price: 99.99,
        image: "images/headphones.jpg",
        category: "electronics",
        description: "High-quality wireless headphones with noise cancellation",
        rating: 4.5,
        reviews: 128,
        inStock: true,
        specifications: {
            brand: "SoundMax",
            color: "Black",
            connectivity: "Bluetooth 5.0",
            batteryLife: "30 hours"
        }
    },
    {
        id: 2,
        name: "Smart Watch",
        price: 199.99,
        image: "images/smartwatch.jpg",
        category: "electronics",
        description: "Latest smart watch with health monitoring features",
        rating: 4.2,
        reviews: 95,
        inStock: true,
        specifications: {
            brand: "TechWear",
            color: "Silver",
            display: "AMOLED",
            batteryLife: "7 days"
        }
    },
    {
        id: 3,
        name: "Bluetooth Speaker",
        price: 79.99,
        image: "images/speaker.jpg",
        category: "electronics",
        description: "Portable bluetooth speaker with 20-hour battery life",
        rating: 4.7,
        reviews: 210,
        inStock: true,
        specifications: {
            brand: "SoundWave",
            color: "Blue",
            power: "20W",
            waterproof: true
        }
    },
    {
        id: 4,
        name: "Men's Casual T-Shirt",
        price: 24.99,
        image: "images/tshirt.jpg",
        category: "clothing",
        description: "Comfortable cotton t-shirt for everyday wear",
        rating: 4.3,
        reviews: 156,
        inStock: true,
        specifications: {
            brand: "FashionCo",
            color: "White",
            material: "100% Cotton",
            sizes: ["S", "M", "L", "XL"]
        }
    },
    {
        id: 5,
        name: "Women's Summer Dress",
        price: 49.99,
        image: "images/dress.jpg",
        category: "clothing",
        description: "Light and breezy summer dress",
        rating: 4.8,
        reviews: 89,
        inStock: true,
        specifications: {
            brand: "StyleHub",
            color: "Floral Print",
            material: "Polyester",
            sizes: ["XS", "S", "M", "L"]
        }
    }
];

const deals = [
    {
        id: 1,
        title: "Summer Sale",
        category: "electronics",
        image: "images/deal1.jpg",
        discount: "30%",
        endDate: "2023-12-31",
        featuredProducts: [1, 2, 3]
    },
    {
        id: 2,
        title: "Fashion Week",
        category: "clothing",
        image: "images/deal2.jpg",
        discount: "25%",
        endDate: "2023-12-31",
        featuredProducts: [4, 5]
    }
];

// Cart data
let cart = [];

// Website data structure
const websiteData = {
    name: "Amazon Clone",
    description: "An e-commerce platform inspired by Amazon",
    version: "1.0.0",
    contact: {
        email: "support@amazonclone.com",
        phone: "+1 (555) 123-4567",
        address: "123 E-commerce Street, Digital City, 10001"
    },
    socialMedia: {
        facebook: "https://facebook.com/amazonclone",
        twitter: "https://twitter.com/amazonclone",
        instagram: "https://instagram.com/amazonclone"
    },
    pages: {
        home: {
            url: "/",
            title: "Home",
            description: "Main landing page with featured products and deals",
            sections: ["hero", "featured-products", "deals"]
        },
        products: {
            url: "/products.html",
            title: "Products",
            description: "All products page",
            sections: ["category-filter", "product-grid"]
        },
        electronics: {
            url: "/electronics.html",
            title: "Electronics",
            description: "Electronics products page",
            sections: ["category-header", "product-grid"]
        },
        clothing: {
            url: "/clothing.html",
            title: "Clothing",
            description: "Clothing products page",
            sections: ["category-header", "product-grid"]
        }
    },
    categories: [
        {
            name: "electronics",
            displayName: "Electronics & Gadgets",
            icon: "fas fa-laptop",
            url: "/electronics.html",
            subcategories: ["Laptops", "Audio", "Wearables", "Smart Home"]
        },
        {
            name: "clothing",
            displayName: "Clothing",
            icon: "fas fa-tshirt",
            url: "/clothing.html",
            subcategories: ["Men", "Women", "Kids"]
        }
    ],
    navigation: {
        main: [
            { name: "Home", url: "/", icon: "fas fa-home" },
            { name: "Products", url: "/products.html", icon: "fas fa-shopping-bag" },
            { name: "Cart", url: "#", icon: "fas fa-shopping-cart" }
        ],
        secondary: [
            { name: "All Categories", url: "/products.html", icon: "fas fa-bars" },
            { name: "Electronics", url: "/electronics.html", icon: "fas fa-laptop" },
            { name: "Clothing", url: "/clothing.html", icon: "fas fa-tshirt" },
            { name: "Today's Deals", url: "#", icon: "fas fa-tag" },
            { name: "Customer Service", url: "#", icon: "fas fa-headset" }
        ]
    }
};

// API Routes
app.get('/api/website', (req, res) => {
    res.json(websiteData);
});

// Get website metadata
app.get('/api/website/metadata', (req, res) => {
    const { name, description, version, contact, socialMedia } = websiteData;
    res.json({ name, description, version, contact, socialMedia });
});

// Get website navigation
app.get('/api/website/navigation', (req, res) => {
    res.json(websiteData.navigation);
});

// Get website pages
app.get('/api/website/pages', (req, res) => {
    res.json(websiteData.pages);
});

// Get website categories
app.get('/api/website/categories', (req, res) => {
    res.json(websiteData.categories);
});

// Products API
app.get('/api/products', (req, res) => {
    res.json(products);
});

app.get('/api/products/:id', (req, res) => {
    const product = products.find(p => p.id === parseInt(req.params.id));
    if (!product) {
        return res.status(404).json({ message: 'Product not found' });
    }
    res.json(product);
});

app.get('/api/products/electronics', (req, res) => {
    const electronicsProducts = products.filter(product => product.category === 'electronics');
    res.json(electronicsProducts);
});

app.get('/api/products/clothing', (req, res) => {
    const clothingProducts = products.filter(product => product.category === 'clothing');
    res.json(clothingProducts);
});

// Search products
app.get('/api/products/search/:query', (req, res) => {
    const query = req.params.query.toLowerCase();
    const results = products.filter(product => 
        product.name.toLowerCase().includes(query) ||
        product.description.toLowerCase().includes(query)
    );
    res.json(results);
});

// Deals API
app.get('/api/deals', (req, res) => {
    res.json(deals);
});

app.get('/api/deals/:id', (req, res) => {
    const deal = deals.find(d => d.id === parseInt(req.params.id));
    if (!deal) {
        return res.status(404).json({ message: 'Deal not found' });
    }
    res.json(deal);
});

// Cart API
app.get('/api/cart', (req, res) => {
    res.json(cart);
});

app.post('/api/cart', (req, res) => {
    const { productId, quantity } = req.body;
    const product = products.find(p => p.id === productId);
    
    if (product) {
        cart.push({
            ...product,
            quantity: quantity || 1
        });
        res.json({ message: 'Product added to cart successfully' });
    } else {
        res.status(404).json({ message: 'Product not found' });
    }
});

app.delete('/api/cart/:productId', (req, res) => {
    const productId = parseInt(req.params.productId);
    cart = cart.filter(item => item.id !== productId);
    res.json({ message: 'Product removed from cart' });
});

// Create HTTPS server
const server = https.createServer(sslOptions, app);

// Start server
server.listen(port, () => {
    console.log(`HTTPS Server running at https://localhost:${port}`);
}); 