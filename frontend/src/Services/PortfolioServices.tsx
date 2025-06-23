import axios from "axios";
import { ErrorHandler } from "../Helpers/ErrorHandler"
import { PortfolioGet, PortfolioPost } from "../Models/Porfolio";

const api = "https://localhost:44351/api/portfolio/"

export const portfolioAddAPI = async (symbol: string) => {
    try {
        const data = await axios.post<PortfolioPost>(api + symbol, {   
            params: {
                symbol: symbol
            }
        });
        return data;
    }catch (e) {
        ErrorHandler(e);
    }
}

export const portfolioDeleteAPI = async (symbol: string) => {
    try {
        const data = await axios.delete<PortfolioPost>(api + symbol, {   
            params: {
                symbol: symbol
            }
        });
        return data;
    }catch (e) {
        ErrorHandler(e);
    }
}

export const portfolioGetAPI = async () => {
    try {
        const data = await axios.delete<PortfolioGet[]>(api);
        return data;
    }catch (e) {
        ErrorHandler(e);
    }
}