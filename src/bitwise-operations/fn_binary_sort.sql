create function [dbo].[fn_binary_sort] (
    @input fn_binary_sort_input readonly
)
returns
@output table (
    id bigint not null,
    value binary(8000) not null,
    rn bigint not null
)
as
begin
        declare @xml xml;

         select @xml = (
                         select *
                           from @input
                        for xml raw,
                         binary base64
                );

    insert into @output ( id, value, rn )
         select id, value, rn
           from fn_binary_sort_clr(@xml);

    return;
end
