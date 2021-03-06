/*
update customer set name = 'Arkansas Furniture Depot' where id = 2
*/

declare
	  @CustomerID		INT
	, @ActiveOnly		BIT

	set @customerid = 2
	set @activeonly = 1

		SELECT
				  i.id
				, i.customer_id
				, cus.name AS customer_name
				, com.name AS company_name
				, i.invoice_number
				, i.description
				, i.source_type_id
				, st.code AS source_type_code
				, i.company_currency_type_id
				, ct1.code AS company_currency_type_code
				, i.company_amount
				, i.customer_currency_type_id
				, ct2.code AS customer_currency_type_code
				, i.customer_amount
				, i.due_date
				, i.expiration_date
				, i.currency_calculation_id
				, i.hedge_date
				, i.close_date
				, i.create_user
				, i.create_date
				, i.update_user
				, i.update_date
		 FROM
 				dbo.UserProfile up WITH (NOLOCK)
		 INNER JOIN 
				dbo.Customer cus WITH (NOLOCK) ON up.customer_id = cus.ID
		 INNER JOIN
				dbo.Company com WITH (NOLOCK) ON cus.Company_ID = com.id
		 INNER JOIN 
				dbo.invoice i WITH (NOLOCK) ON cus.id = i.Customer_ID
		 INNER JOIN 
				dbo.source_type st WITH (NOLOCK) ON i.source_type_id = st.id
		 INNER JOIN 
				dbo.currency_type ct1 WITH (NOLOCK) ON i.company_currency_type_id = ct1.id
		 INNER JOIN 
				dbo.currency_type ct2 WITH (NOLOCK) ON i.customer_currency_type_id = ct2.id
		 WHERE
				cus.id = @CustomerID
		  AND	((@ActiveOnly = 1 AND i.Close_Date IS NULL)
		   OR	(@ActiveOnly = 0))
