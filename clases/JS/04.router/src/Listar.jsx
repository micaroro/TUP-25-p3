import { Link } from 'react-router-dom';
import { useContacts } from './hooks/useContacts';

export function Listar() {
  const { contacts, deleteContact } = useContacts();

  const handleDelete = (id, nombre) => {
    if (window.confirm(`¿Estás seguro de que quieres eliminar a ${nombre}?`)) {
      deleteContact(id);
    }
  };

  return (
    <div className="list-container">
      <div className="list-header">
        <h1>📋 Lista de Contactos</h1>
        <Link to="/editar/nuevo" className="btn btn-primary">
          ➕ Agregar Contacto
        </Link>
      </div>

      {contacts.length === 0 ? (
        <div className="empty-state">
          <p>No hay contactos registrados</p>
          <Link to="/editar/nuevo" className="btn btn-primary">
            Agregar el primer contacto
          </Link>
        </div>
      ) : (
        <div className="contacts-grid">
          {contacts.map((contact) => (
            <div key={contact.id} className="contact-card">
              <div className="contact-info">
                <h3>{contact.nombre}</h3>
                <p className="contact-age">Edad: {contact.edad} años</p>
              </div>
              <div className="contact-actions">
                <Link 
                  to={`/editar/${contact.id}`} 
                  className="btn btn-secondary btn-sm"
                >
                  ✏️ Editar
                </Link>
                <button
                  onClick={() => handleDelete(contact.id, contact.nombre)}
                  className="btn btn-danger btn-sm"
                >
                  🗑️ Eliminar
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
