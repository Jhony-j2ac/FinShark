import { createContext, useContext, useEffect, useState } from "react";
import { UserProfile } from "../Models/User";
import { useNavigate } from "react-router";
import { loginAPI, registerAPI } from "../Services/AuthServices";
import { toast } from "react-toastify";
import axios from "axios";

type UserContextType = {
    user: UserProfile | null;
    token: string | null;
    registerUser: (email: string, username: string, password: string) => void;
    loginUser: (username: string, password: string) => void;
    logoutUser: () => void;
    isLoggedIn: () => boolean;
};

type Props = {
    children: React.ReactNode;
};

const UserContext = createContext<UserContextType>({} as UserContextType);

// Helper functions for cookies
function setCookie(name: string, value: string, days = 7) {
    const expires = new Date(Date.now() + days * 864e5).toUTCString();
    document.cookie = `${name}=${encodeURIComponent(value)}; expires=${expires}; path=/; SameSite=Strict; Secure`;
}

function getCookie(name: string) {
    return document.cookie.split('; ').reduce((r, v) => {
        const parts = v.split('=');
        return parts[0] === name ? decodeURIComponent(parts[1]) : r
    }, "");
}

function deleteCookie(name: string) {
    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
}

export const UserProvider = ({ children }: Props) => {
    const navigate = useNavigate();
    const [token, setToken] = useState<string | null>(null);
    const [user, setUser] = useState<UserProfile | null>(null);
    const [isReady, setIsReady] = useState<boolean>(false);

    useEffect(() => {
        const userStr = getCookie("user");
        const tokenStr = getCookie("token");
        if (userStr && tokenStr) {
            setUser(JSON.parse(userStr));
            setToken(tokenStr);
            axios.defaults.headers.common["Authorization"] = "Bearer " + tokenStr;
        }
        setIsReady(true);
    }, []);

    const registerUser = async (email: string, username: string, password: string) => {
        await registerAPI(email, username, password).then((res) => {
            if (res) {
                setCookie("token", res?.data.token);
                const userObj: UserProfile = {
                    userName: res?.data.userName,
                    email: res?.data.email
                };
                setCookie("user", JSON.stringify(userObj));
                setToken(res?.data.token);
                setUser(userObj!);
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
                setCookie("token", res?.data.token);
                const userObj: UserProfile = {
                    userName: res?.data.userName,
                    email: res?.data.email
                };
                setCookie("user", JSON.stringify(userObj));
                setToken(res?.data.token);
                setUser(userObj!);
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

    const logoutUser = () => {
        deleteCookie("user");
        deleteCookie("token");
        setUser(null);
        setToken("");
        navigate("/");
    };

    return (
        <UserContext.Provider value={{ loginUser, user, token, logoutUser, isLoggedIn, registerUser }}>
            {isReady ? children : null}
        </UserContext.Provider>
    );
};

export const useAuthCookie = () => useContext(UserContext);