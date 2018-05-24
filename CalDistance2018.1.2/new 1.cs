public void Cal(double res, int key, List<double> list,int count,int totalKey)
{
	if(count==k-r)
	{
		for(int i=0;i<4;i++)
		{
			markov[totalKey+i]=res*list[key+i];
		}
		return;
	}
	else
	{
		for(int i=0;i<4;i++)
		{
			double tmp=res*list[key+i]
		}
		return;
	}
	
}

public void Cal(int rKey,double res,List<double> list,int count,int key,int count)
{
	if(count==k-r)
	{
		for(int i=0;i<4;i+=4)
		{
			markov[key+i]=res*list[rKey+i];
		}
		
	}
	for(int i=0;i<4;i+=4)
	{				
        res*=list[rkey];
		key=key*4+i;
		Cal(rkey-=(rkey/4^(r-1)*4),res,list,count+1);		
	}
}