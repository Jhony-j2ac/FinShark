import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router'
import { CompanyProfile } from '../../company';
import { getCompanyProfile } from '../../api';
import Sidebar from '../../Components/Sidebar/Sidebar';
import CompanyDashboard from '../../Components/CompanyDashboard/CompanyDashboard';
import Tile from '../../Components/Tile/Tile';
import Spinner from '../../Components/Spinner/Spinner';
import CompFinder from '../../Components/CompFinder/CompFinder';
import TenKFinder from '../../Components/TenKFinder/TenKFinder';

interface Props {

}


const CompanyPage : React.FC = (props: Props) :JSX.Element => {
    let {ticker} = useParams();
    const [company, setComapny] = useState<CompanyProfile>()
    useEffect(() => {
        const getProfileInit = async() =>{
            const result = await getCompanyProfile(ticker!);
            setComapny(result?.data[0]);
        }
        getProfileInit();
    }, []);
  return (
    <>
        {
            company ? 
            <div className="w-full relative flex ct-docs-disable-sidebar-content overflow-x-hidden">
            <Sidebar></Sidebar>
            <CompanyDashboard ticker={ticker!}>
                <Tile title="Company Name" subTitle={company.companyName} />
                <Tile title="Price" subTitle={'$' + company.price.toString()} />
                <Tile title="DCF" subTitle={'$' + company.dcf.toString()} />
                <Tile title="Sector" subTitle={company.sector} />
                <CompFinder ticker={company.symbol}/>
                <TenKFinder ticker={company.symbol} />
                <p className='bg-white shadow rounded text-gray-900 mt-1 m-4'>
                    {company.description}
                </p>
            </CompanyDashboard>

        </div>
            : <Spinner />
        }
    </>
  )
}

export default CompanyPage