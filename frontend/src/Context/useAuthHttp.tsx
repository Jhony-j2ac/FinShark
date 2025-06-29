import { createContext, useContext, useEffect, useMemo, useState, useCallback } from "react";
import { UserProfile } from "../Models/User";
import { useNavigate } from "react-router";
import { loginAPI, logoutAPI, registerAPI } from "../Services/AuthServices";
import { toast } from "react-toastify";
import axios, { AxiosInstance } from "axios";

type UserContextType = {
    user: UserProfile | null;
    registerUser: (email: string, username: string, password: string) => void;
    loginUser: (username: string, password: string) => void;
    logoutUser: () => void;
    isLoggedIn: () => boolean;
    authAxios: AxiosInstance;

};

type Props = {
    children: React.ReactNode;
};

const UserContext = createContext<UserContextType>({} as UserContextType);

// Local storage helpers
function setLocalUser(user: UserProfile) {
    localStorage.setItem("user", JSON.stringify(user));
}

function getLocalUser(): UserProfile | null {
    const userStr = localStorage.getItem("user");
    return userStr ? JSON.parse(userStr) : null;
}

function removeLocalUser() {
    localStorage.removeItem("user");
}


export const UserProvider = ({ children }: Props) => {
    const navigate = useNavigate();
    const [user, setUser] = useState<UserProfile | null>(null);
    const [isReady, setIsReady] = useState<boolean>(false);

    // Axios instance with refresh logic
    

    useEffect(() => {
        const userStr =  getLocalUser();
        if (userStr) {
            setUser(userStr);
        }
        setIsReady(true);
    }, []);

    const registerUser = async (email: string, username: string, password: string) => {
        await registerAPI(email, username, password).then((res) => {
            if (res) {
                const userObj: UserProfile = {
                    userName: res?.data.userName,
                    email: res?.data.email
                };
                setUser(userObj!);
                setLocalUser(userObj!);
                toast.success("Login Success!");
                navigate("/search");
            }
        }).catch((e) => {
            toast.warning("Server error occurred");
            console.log(e);
        });
    };

    const loginUser = async (username: string, password: string) => {
        await loginAPI(username, password).then((res) => {
            if (res) {
                const userObj: UserProfile = {
                    userName: res?.data.userName,
                    email: res?.data.email
                };
                setUser(userObj!);
                setLocalUser(userObj);
                toast.success("Login Success!");
                navigate("/search");
            }
        }).catch((e) => {
            toast.warning("Server error occurred");
            console.log(e);
        });
    };

    const isLoggedIn = () => {
        return !!user;
    };

    const logoutUser = useCallback(() => {
        logoutAPI(user!.userName)
            .then((res: any) => {
                removeLocalUser();
                setUser(null);
                navigate("/");
            })
            .catch((e: any) => {
                console.log(e);
                removeLocalUser();
                setUser(null);
                navigate("/");
            });
    }, [user, navigate]);


     // Axios instance with refresh logic, now inside the context
    const authAxios = useMemo(() => {
        const instance = axios.create({
        //baseURL: "https://localhost:44351/api/",
            withCredentials: true,
        });
        /* instance.interceptors.request.use(
            (config) => {
                if (token) {
                    config.headers = config.headers || {};
                    config.headers["Authorization"] = `Bearer ${token}`;
                }
                return config;
            },
            (error) => Promise.reject(error)
            );
        */
        instance.interceptors.response.use(
            (response) => response,
            async (error) => {
                const originalRequest = error.config;
                if (
                    error.response &&
                    error.response.status === 401 &&
                    !originalRequest._retry
                ) {
                    originalRequest._retry = true;
                    try {
                    // Intenta refrescar el token usando la cookie HttpOnly
                    const res = await axios.post(
                        "https://localhost:44351/api/Account/refresh",
                        {},
                        { withCredentials: true }
                    );
                    /* setToken(res.data.token);
                    // Reintenta la petición original con el nuevo token
                    originalRequest.headers["Authorization"] = `Bearer ${res.data.token}`; */
                    return instance(originalRequest);
                    } catch (refreshError) {
                        logoutUser();
                        return Promise.reject(refreshError);
                    }
                }
                return Promise.reject(error);
            }
        );

        return instance;
    }, [logoutUser]);


    return (
        <UserContext.Provider value={{ loginUser, user, logoutUser, isLoggedIn, registerUser, authAxios }}>
            {isReady ? children : null}
        </UserContext.Provider>
    );
};

export const useAuthHttp = () => useContext(UserContext);
