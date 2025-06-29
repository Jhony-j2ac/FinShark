import React, { useEffect } from 'react'
import StockCommentForm from './StockCommentForm/StockCommentForm';
import { useCommentService } from '../../Services/CommentService';
import { toast } from 'react-toastify';
import { CommentGet } from '../../Models/Comment';
import { set } from 'react-hook-form';
import { get } from 'http';
import StockCommentList from '../StockCommentList/StockCommentList';
import Spinner from '../Spinner/Spinner';

type Props = {
    stockSymbol: string;
}



const StockComment = ({stockSymbol}: Props) => {
    const {commentGetAPI, commentPostAPI} = useCommentService();
    const [comments, setComments] = React.useState<CommentGet[] | null>([]);
    const [loading, setLoading] = React.useState<boolean>(false);

    useEffect(() => {
        getComments();
    }, []);

    const handleComment = (e : CommentFormInputs) => {
        commentPostAPI(e.title, e.content, stockSymbol)
        .then( (res : any) => {
            if(typeof res !== "undefined") {
                toast.success("Comment posted successfully");
                getComments();
            }else{
                toast.error("Failed to post comment");
            }
        }).catch( (err : any) => {
            toast.error("An error occurred while posting the comment");
        });
    }

    const getComments = async () =>{
        setLoading(true);
        commentGetAPI(stockSymbol).then((res) => {
            setComments(res?.data!);
            setLoading(false);
        });
    }
  return (
    <div className='flex flex-col items-center justify-center w-full h-full p-4'>
        {loading ? <Spinner /> : <StockCommentList comments={comments!} /> }
        <StockCommentForm symbol={stockSymbol}  handleComment={handleComment} />
        
    </div>
  )
}

export default StockComment

export type CommentFormInputs = {
    title: string;
    content: string;
}