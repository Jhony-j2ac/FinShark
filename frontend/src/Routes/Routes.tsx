import { createBrowserRouter } from "react-router";
import App from "../App";
import HomePage from "../Pages/HomePage/HomePage";
import SearchPage from "../Pages/SearchPage/SearchPage";
import CompanyPage from "../Pages/CompanyPage/CompanyPage";
import CompanyProfile from "../Components/CompanyProfile/CompanyProfile";
import IncomeStatement from "../Components/IncomeStatement/IncomeStatement";
import DesignPage from "../Pages/DesignPage/DesignPage";
import BalanceSheet from "../Components/BalanceSheet/BalanceSheet";
import CashflowStatement from "../Components/CashFlowStatement/CashflowStatement";

 export const router = createBrowserRouter([
    {
        path: "/",
        element: <App></App>,
        children: [
            { path: "", element: <HomePage></HomePage>},
            { path: "search", element: <SearchPage></SearchPage>},
            { path: "design-guide", element: <DesignPage />},
            { 
                path: "company/:ticker", 
                element: <CompanyPage></CompanyPage>,
                children: [
                    { path: "company-profile", element: <CompanyProfile />},
                    { path: "income-statement", element: <IncomeStatement/>},
                    { path: "balance-sheet", element: <BalanceSheet/>},
                    { path: "cashflow-statement", element: <CashflowStatement/>},
                ] 
            },
        ]
    }
 ])