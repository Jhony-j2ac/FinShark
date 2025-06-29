import { useAuthHttp } from "../Context/useAuthHttp";
import { ErrorHandler } from "../Helpers/ErrorHandler";
import { PortfolioGet, PortfolioPost } from "../Models/Porfolio";

const api = "https://localhost:44351/api/Portfolio";

export const usePortfolioService = () => {
    const { authAxios } = useAuthHttp();

    const portfolioAddAPI = async (symbol: string) => {
        try {
            const data = await authAxios.post<PortfolioPost>(api + "/" + symbol, {}, {
                params: { symbol },
                withCredentials: true
            });
            return data;
        } catch (e) {
            ErrorHandler(e);
        }
    };

    const portfolioDeleteAPI = async (symbol: string) => {
        try {
            const data = await authAxios.delete<PortfolioPost>(api + "/" + symbol, {
                withCredentials: true
            });
            return data;
        } catch (e) {
            ErrorHandler(e);
        }
    };

    const portfolioGetAPI = async () => {
        try {
            const data = await authAxios.get<PortfolioGet[]>(api, {
                withCredentials: true
            });
            return data;
        } catch (e) {
            ErrorHandler(e);
        }
    };

    return { portfolioAddAPI, portfolioDeleteAPI, portfolioGetAPI };
};