import React, { useEffect, useState } from 'react'
import { CompanyCompData } from '../../company';
import { getCompData } from '../../api';
import CompFinderItem from './CompFinderItem/CompFinderItem';

type Props = {
    ticker: string;
}

const CompFinder = ({ticker}: Props) => {
    const [companyData, setCompanyData] = useState<CompanyCompData>();
    useEffect(() => {
        const getComps = async () => {
            const data = await getCompData(ticker);
            setCompanyData(data?.data[0])
        };
        getComps();
    }, [ticker]);
  return (
    <div className='inline-flex  rounded medium shadow-sm m-4'>
        {companyData?.peersList.map( (ticker) => {
          return   <CompFinderItem ticker={ticker}/>;
        })}
    </div>
  )
}

export default CompFinder