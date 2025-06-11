import React from 'react'
import { TestDataCompany, testIncomeStatementData } from './TestData'
import { CompanyIncomeStatement } from '../../company'

const data = testIncomeStatementData

type Props = {
    configs : any; 
    data : any;
}


const Table = ({configs, data}: Props) => {
    const renderedRows = data.map((company : any) => {
        return (
            <tr key={company.cik}>
                {configs.map( (val: any) => {
                    return(
                        <td className='p-4 whitespace-nowrap text-sm font-normal text-gray-900 '>{val.render(company)}</td>
                    );
                })}
            </tr>
        )
    });
    const renderedHeaders = configs.map((config :any) =>  {
        return (
            <th className='p-4 text-left text-xs font-medium text-gray-500 uppercase'
            key={config.label}>
                {config.label}
            </th>
        );
    });
    return (
        <div className='bg-white shadow rounded-lg p-4 sm:p-6 xl:p-8 overflow-x-auto'>
            <table className='min-w-full table-auto'>
                <thead className='border-t-gray-100 border-t-2 m-5 '>
                    {renderedHeaders}
                </thead>
                <tbody>
                    {renderedRows}
                </tbody>
            </table> 
        </div>
    )
}

export default Table