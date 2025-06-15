import { useState, useEffect } from 'react';

const STORAGE_KEY = 'contactos';

// Hook personalizado para manejar contactos en localStorage
export function useContacts() {
  const [contacts, setContacts] = useState([]);

  // Cargar contactos del localStorage al inicializar
  useEffect(() => {
    loadContacts();
  }, []);

  // Cargar contactos desde localStorage
  const loadContacts = () => {
    try {
      const stored = localStorage.getItem(STORAGE_KEY);
      if (stored) {
        const parsedContacts = JSON.parse(stored);
        setContacts(parsedContacts);
      } else {
        // Datos iniciales si no hay nada en localStorage
        const initialContacts = [
          { id: 1, nombre: 'Alejandro', edad: 25 },
          { id: 2, nombre: 'Franco', edad: 30 },
          { id: 3, nombre: 'Maira', edad: 28 },
          { id: 4, nombre: 'Mirta', edad: 45 },
          { id: 5, nombre: 'Elidio', edad: 35 }
        ];
        setContacts(initialContacts);
        localStorage.setItem(STORAGE_KEY, JSON.stringify(initialContacts));
      }
    } catch (error) {
      console.error('Error al cargar contactos:', error);
      setContacts([]);
    }
  };

  // Guardar contactos en localStorage
  const saveContacts = (updatedContacts) => {
    try {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(updatedContacts));
      setContacts(updatedContacts);
    } catch (error) {
      console.error('Error al guardar contactos:', error);
    }
  };

  // Obtener contacto por ID
  const getContactById = (id) => {
    return contacts.find(contact => contact.id === parseInt(id));
  };

  // Obtener contacto por nombre
  const getContactByName = (nombre) => {
    return contacts.find(contact => 
      contact.nombre.toLowerCase() === nombre.toLowerCase()
    );
  };

  // Agregar nuevo contacto
  const addContact = (contactData) => {
    const newId = Math.max(...contacts.map(c => c.id), 0) + 1;
    const newContact = {
      id: newId,
      nombre: contactData.nombre.trim(),
      edad: parseInt(contactData.edad) || 0
    };
    
    const updatedContacts = [...contacts, newContact];
    saveContacts(updatedContacts);
    return newContact;
  };

  // Actualizar contacto existente
  const updateContact = (id, contactData) => {
    const updatedContacts = contacts.map(contact => {
      if (contact.id === parseInt(id)) {
        return {
          ...contact,
          nombre: contactData.nombre.trim(),
          edad: parseInt(contactData.edad) || 0
        };
      }
      return contact;
    });
    
    saveContacts(updatedContacts);
    return updatedContacts.find(c => c.id === parseInt(id));
  };

  // Eliminar contacto
  const deleteContact = (id) => {
    const updatedContacts = contacts.filter(contact => contact.id !== parseInt(id));
    saveContacts(updatedContacts);
  };

  // Verificar si existe un contacto con el mismo nombre (excluyendo el ID actual)
  const isNameTaken = (nombre, excludeId = null) => {
    return contacts.some(contact => 
      contact.nombre.toLowerCase() === nombre.toLowerCase() && 
      contact.id !== excludeId
    );
  };

  return {
    contacts,
    getContactById,
    getContactByName,
    addContact,
    updateContact,
    deleteContact,
    isNameTaken,
    loadContacts
  };
}
