import { Outlet } from 'react-router';
import './App.css';
import 'react-toastify/dist/ReactToastify.css';
import Navbar from './Components/Navbar/Navbar';
import { ToastContainer } from 'react-toastify';
import { UserProvider } from './Context/useAuthHttp';

function App() {
  return (
    <>
        <UserProvider>
            <Navbar></Navbar>
            <Outlet></Outlet>
            <ToastContainer></ToastContainer>
        </UserProvider>
    </>
  );
}

export default App;
