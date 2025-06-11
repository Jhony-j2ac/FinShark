import React from 'react'
import {v4 as uuid} from 'uuid'

type Props = {
    data : any,
    config : any,
}

const RadioList : React.FC<Props> = ({config, data}: Props) : JSX.Element => {
    const renderedCell = config.map((cell : any) => {
        return (
            <li className='py-3 sm:py-4 ' key={uuid()}>
                <div className='flex items-center space-x-4'>
                    <div className='flex-1 min-w-0'>
                        <p className="text-sm font-medium text-gray-900 truncate">
                            {cell.label}
                        </p>
                        <div className='text-sm text-gray-500 truncate'>
                            {cell.subTitle && cell.subTitle}
                        </div>
                    </div>
                    <div className="inline-flex items-center text-base font-semibold text-gray-900">
                        {cell.render(data)}
                    </div>
                </div>
            </li>
        );
    });
  return (
    <div className='bg-white shadow rounded-lg ml-4 mt-4 mb-4 p-4 sm:p-6 h-full'>
        <ul className="divide-y divide-gray-200">
            {renderedCell}
        </ul>
    </div>
  )
} 

export default RadioList;