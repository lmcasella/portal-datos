import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axiosConfig';
import { useAuthStore } from '../store/authStore';

export const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  
  const navigate = useNavigate();
  // Traemos la función setLogin de nuestro estado global (Zustand)
  const setLogin = useAuthStore((state) => state.setLogin);

  const handleSubmit = async (e) => {
    e.preventDefault(); // Evitamos que la página se recargue
    setError('');

    try {
      // Le pegamos al endpoint en .NET (usando la instancia de Axios)
      const response = await api.post('/auth/login', {
        username: username,
        password: password
      });

      // Si sale bien, guardamos el token globalmente
      setLogin(response.data.token);
      
      // Y redirigimos al panel principal
      navigate('/dashboard');
    } catch (err) {
      setError('Credenciales incorrectas o error en el servidor.');
    }
  };

  return (
    <div style={{ padding: '50px', maxWidth: '400px', margin: '0 auto' }}>
      <h2>Iniciar Sesión</h2>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      
      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '15px' }}>
        <div>
          <label>Usuario:</label>
          <input 
            type="text" 
            value={username} 
            onChange={(e) => setUsername(e.target.value)} 
            required 
            style={{ width: '100%', padding: '8px' }}
          />
        </div>
        <div>
          <label>Contraseña:</label>
          <input 
            type="password" 
            value={password} 
            onChange={(e) => setPassword(e.target.value)} 
            required 
            style={{ width: '100%', padding: '8px' }}
          />
        </div>
        <button type="submit" style={{ padding: '10px', cursor: 'pointer' }}>Ingresar</button>
      </form>
    </div>
  );
};