import React, { useEffect, useId, useState } from 'react'
import { CompanyTenK } from '../../company';
import { getTenK } from '../../api';
import TenkFinderItem from './TenkFinderItem/TenkFinderItem';
import Spinner from '../Spinner/Spinner';

type Props = {
    ticker: string;
}

const TenKFinder = ({ticker}: Props) => {
    const [companyData, setCompanyData] = useState<CompanyTenK[]>();
    useEffect(() =>{
        const getTenkData = async() => {
            const data  = await getTenK(ticker);
            setCompanyData(data?.data);
        };
        getTenkData();
    }, []);
    const idGen = useId();
  return (
    <div className='inline-flex rounded-md shadow-sm m-4'>
        {companyData? (
            companyData.slice(0, 5).map((tenK) => {
                return <TenkFinderItem tenK={tenK} key={tenK.finalLink} />
            })
        ) : 
        (
            <Spinner />
        )}
    </div>
  )
}

export default TenKFinder