import { useParams, useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { useContacts } from './hooks/useContacts';

export function Editar() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { getContactById, addContact, updateContact, isNameTaken } = useContacts();

  const isNew = id === 'nuevo';
  const contactId = isNew ? null : parseInt(id);

  // Estados para el formulario
  const [formData, setFormData] = useState({
    nombre: '',
    edad: '',
  });
  const [errors, setErrors] = useState({});
  const [isLoading, setIsLoading] = useState(false);

  // Cargar datos del contacto si estamos editando
  useEffect(() => {
    if (!isNew && contactId) {
      const contact = getContactById(contactId);
      if (contact) {
        setFormData({
          nombre: contact.nombre,
          edad: contact.edad.toString(),
        });
      } else {
        // Si no existe el contacto, redirigir a la lista
        navigate('/listar');
      }
    }
  }, [contactId, isNew, getContactById, navigate]);

  // Manejar cambios en los inputs
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
    
    // Limpiar errores cuando el usuario empiece a escribir
    if (errors[name]) {
      setErrors((prev) => ({
        ...prev,
        [name]: '',
      }));
    }
  };

  // Validar formulario
  const validateForm = () => {
    const newErrors = {};

    // Validar nombre
    if (!formData.nombre.trim()) {
      newErrors.nombre = 'El nombre es obligatorio';
    } else if (formData.nombre.trim().length < 2) {
      newErrors.nombre = 'El nombre debe tener al menos 2 caracteres';
    } else if (isNameTaken(formData.nombre.trim(), contactId)) {
      newErrors.nombre = 'Ya existe un contacto con este nombre';
    }

    // Validar edad
    if (!formData.edad.trim()) {
      newErrors.edad = 'La edad es obligatoria';
    } else {
      const edad = parseInt(formData.edad);
      if (isNaN(edad) || edad < 0) {
        newErrors.edad = 'La edad debe ser un número positivo';
      } else if (edad > 120) {
        newErrors.edad = 'La edad no puede ser mayor a 120 años';
      }
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  // Validar y enviar formulario
  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    setIsLoading(true);
    
    try {
      const contactData = {
        nombre: formData.nombre.trim(),
        edad: parseInt(formData.edad),
      };

      if (isNew) {
        addContact(contactData);
      } else {
        updateContact(contactId, contactData);
      }

      // Navegar a la página listar
      navigate('/listar');
    } catch (error) {
      console.error('Error al guardar contacto:', error);
      setErrors({ general: 'Error al guardar el contacto. Inténtalo de nuevo.' });
    } finally {
      setIsLoading(false);
    }
  };

  // Cancelar y regresar a la página anterior
  const handleCancel = () => {
    navigate('/listar');
  };

  return (
    <div className="form-container">
      <h1>
        {isNew ? '➕ Agregar Contacto' : `✏️ Editando ${formData.nombre || 'Contacto'}`}
      </h1>

      <form onSubmit={handleSubmit}>
        {errors.general && (
          <div className="form-error general-error">
            {errors.general}
          </div>
        )}

        <div className="form-group">
          <label htmlFor="nombre" className="form-label">
            Nombre: *
          </label>
          <input
            type="text"
            id="nombre"
            name="nombre"
            value={formData.nombre}
            onChange={handleChange}
            className={`form-input ${errors.nombre ? 'error' : ''}`}
            placeholder="Ingresa el nombre completo"
            disabled={isLoading}
          />
          {errors.nombre && <div className="form-error">{errors.nombre}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="edad" className="form-label">
            Edad: *
          </label>
          <input
            type="number"
            id="edad"
            name="edad"
            value={formData.edad}
            onChange={handleChange}
            min="0"
            max="120"
            className={`form-input ${errors.edad ? 'error' : ''}`}
            placeholder="Ingresa la edad"
            disabled={isLoading}
          />
          {errors.edad && <div className="form-error">{errors.edad}</div>}
        </div>

        <div className="form-buttons">
          <button 
            type="submit" 
            className="btn btn-primary"
            disabled={isLoading}
          >
            {isLoading ? 'Guardando...' : (isNew ? 'Agregar' : 'Actualizar')}
          </button>

          <button
            type="button"
            onClick={handleCancel}
            className="btn btn-secondary"
            disabled={isLoading}
          >
            Cancelar
          </button>
        </div>
      </form>
    </div>
  );
}
