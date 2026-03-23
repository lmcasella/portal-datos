import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axiosConfig';
import { useAuthStore } from '../store/authStore';

export const Dashboard = () => {
  const [pagos, setPagos] = useState([]);
  const [error, setError] = useState('');
  
  const navigate = useNavigate();
  const setLogout = useAuthStore((state) => state.setLogout);

  useEffect(() => {
    // Apenas carga el componente, vamos a buscar los datos
    const fetchReporte = async () => {
      try {
        // Fijate que acá NO pasamos el token a mano. 
        // El interceptor de Axios que hicimos antes lo inyecta solo.
        const response = await api.get('/reportes/pagos');
        setPagos(response.data);
      } catch (err) {
        setError('Error al cargar los datos. ¿El token expiró?');
      }
    };

    fetchReporte();
  }, []);

  const handleCerrarSesion = () => {
    setLogout(); // Limpiamos Zustand y el LocalStorage
    navigate('/'); // Volvemos al login
  };

  return (
    <div style={{ padding: '30px' }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h2>Reporte de Pagos (Dashboard Privado)</h2>
        <button onClick={handleCerrarSesion} style={{ padding: '8px 15px', background: '#ff4c4c', color: 'white', border: 'none', cursor: 'pointer' }}>
          Cerrar Sesión
        </button>
      </div>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      <table style={{ width: '100%', marginTop: '20px', borderCollapse: 'collapse' }}>
        <thead>
          <tr style={{ background: '#f4f4f4', textAlign: 'left' }}>
            <th style={{ padding: '10px', border: '1px solid #ddd' }}>Nº Boleta</th>
            <th style={{ padding: '10px', border: '1px solid #ddd' }}>Contribuyente</th>
            <th style={{ padding: '10px', border: '1px solid #ddd' }}>Monto</th>
            <th style={{ padding: '10px', border: '1px solid #ddd' }}>Fecha Cobro</th>
          </tr>
        </thead>
        <tbody>
          {pagos.map((pago, index) => (
            <tr key={index}>
              <td style={{ padding: '10px', border: '1px solid #ddd' }}>{pago.numeroBoleta}</td>
              <td style={{ padding: '10px', border: '1px solid #ddd' }}>{pago.nombreCompleto}</td>
              <td style={{ padding: '10px', border: '1px solid #ddd' }}>${pago.montoCobrado.toFixed(2)}</td>
              <td style={{ padding: '10px', border: '1px solid #ddd' }}>{new Date(pago.fechaCobro).toLocaleDateString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};