import { useState } from 'react';
import { Editar } from './Editar';
import { Listar } from './Listar';
import { Routes, Route, Link } from 'react-router-dom';
import './App.css';

function App() {
  return (
    <>
      <nav className="nav-container">
        <div className="nav-links">
          <Link to="/listar" className="nav-link">
            📋 Contactos
          </Link>
          <span className="nav-separator">|</span>
          <Link to="/editar/nuevo" className="nav-link">
            ➕ Agregar
          </Link>
        </div>
      </nav>

      <main>
        <Routes>
          <Route path="/" element={<Listar />} />
          <Route path="/listar" element={<Listar />} />
          <Route path="/editar/:id" element={<Editar />} />
          <Route
            path="*"
            element={
              <div className="error-page">
                <h1>404</h1>
                <p>Página no encontrada</p>
                <Link
                  to="/listar"
                  className="nav-link"
                  style={{ marginTop: '1rem' }}
                >
                  Volver al inicio
                </Link>
              </div>
            }
          />
        </Routes>
      </main>
    </>
  );
}

export default App;
