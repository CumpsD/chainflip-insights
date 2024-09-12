# GraphQL Queries

## Burn

```graphql
{
    allBurns(orderBy: TIMESTAMP_DESC) {
        nodes {
            timestamp
            amount
            eventByEventId {
                blockId
            }
        }
    }
}
```

## CFE Versions

```graphql
{
    allCfeVersions(orderBy: ID_DESC) {
        edges {
            node {
                id
                validatorsByCfeVersionId {
                    edges {
                        node {
                            accountByAccountId {
                                idSs58
                            }
                            lastHeartbeatBlockId
                        }
                    }
                }
            }
        }
    }
}
```

## Daily LP Overview

```graphql
{
    allAccounts {
        nodes {
            idSs58
            limitOrders: limitOrderFillsByLiquidityProviderId(
                filter: {
                    blockTimestamp: {
                        greaterThanOrEqualTo: \"TIME_FROM\", 
                        lessThan: \"TIME_TO\"
                    }
                }
            ) {
                groupedAggregates(groupBy: BLOCK_TIMESTAMP_TRUNCATED_TO_DAY) {
                    sum {
                        feesEarnedValueUsd
                        filledAmountValueUsd
                    }
                    keys
                }
            }
            rangeOrders: rangeOrderFillsByLiquidityProviderId(
                filter: {
                    blockTimestamp: {
                        greaterThanOrEqualTo: \"TIME_FROM\", 
                        lessThan: \"TIME_TO\"
                    }
                }
            ) {
                groupedAggregates(groupBy: BLOCK_TIMESTAMP_TRUNCATED_TO_DAY) {
                    sum {
                        quoteFeesEarnedValueUsd
                        quoteFilledAmountValueUsd
                        baseFeesEarnedValueUsd
                        baseFilledAmountValueUsd
                    }
                    keys
                }
            }
        }
    }
}
```

## Epoch

```graphql
{
    allEpoches(orderBy: ID_DESC, first: 10, filter: {
        id: { greaterThanOrEqualTo: LAST_ID }
    }) {
        edges {
            node {
                id
                bond # MAB, divide by 10^18
                totalBonded # divide by 10^18
                startBlockId
                blockByStartBlockId {
                    timestamp
                }
                authorityMembershipsByEpochId {
                    edges {
                        node {
                            validatorId
                            validatorByValidatorId {
                                accountByAccountId {
                                    idSs58
                                }
                            }
                            bid
                            reward
                        }
                    }
                }
            }
        }
    }
}
```

## Funding

```graphql
{
    allAccountFundingEvents(orderBy: ID_ASC, first: 100, filter: {
        and: {
            id: { greaterThan: LAST_ID }
            type: { equalTo: FUNDED }
        }
    }) {
        edges {
            node {
                id
                
                amount
                epochId
                accountByAccountId {
                    alias
                    idSs58
                    role
                }
                eventByEventId {
                    blockByBlockId {
                        timestamp
                    }
                }
            }
        }
    }
}
```

## Incoming Liquidity

```graphql
{
    allLiquidityDeposits(orderBy: ID_ASC, first: 100, filter: {
        id: { greaterThan: LAST_ID }
        }) {
        edges {
            node {
                id
                depositAmount
                depositValueUsd
                block: blockByBlockId {
                    timestamp
                }
                channel: liquidityDepositChannelByLiquidityDepositChannelId {
                    issuedBlockId
                    chain
                    asset
                    channelId
                    depositAddress
                    isExpired
                }
            }
        }
    }
}
```

## Outgoing Liquidity

```graphql
{
    allLiquidityWithdrawals(orderBy: ID_DESC, first: 100, filter: {
        id: { greaterThan: LAST_ID }
        }) {
        edges {
            node {
                id
                amount
                valueUsd
                chain
                asset
                block: blockByBlockId {
                    id
                    timestamp
                }        
            }
        }
    }
}
```

## Past Volume

```graphql
{
    allPoolSwaps(orderBy: ID_DESC, filter: {
        assetSwappedBlockTimestamp: { 
            greaterThanOrEqualTo: \"TIME_FROM\", lessThanOrEqualTo: \"TIME_TO\"
            }
        }) {
        groupedAggregates(groupBy: [FROM_ASSET, TO_ASSET]) {
            fromAssetToAsset: keys
            sum {
                toValueUsd
                liquidityFeeValueUsd
            }
        }
    }
}
```

## Past Fees

```graphql
{
    allSwaps(orderBy: ID_DESC, filter: {
        swapScheduledBlockTimestamp: {
            greaterThanOrEqualTo: \"TIME_FROM\", lessThanOrEqualTo: \"TIME_TO\"
            }
        }) {
        nodes {
            sourceAsset
            sourceChain

            swapInputAmount
            swapInputValueUsd

            intermediateAmount
            intermediateValueUsd

            swapOutputAmount
            swapOutputValueUsd

            destinationAsset
            destinationChain
        }
    }
}
```

## Redemptions

```graphql
{
    allAccountFundingEvents(orderBy: ID_ASC, first: 100, filter: {
        and: {
            id: { greaterThan: LAST_ID }
            type: { equalTo: REDEEMED }
        }
    }) {
        edges {
            node {
                id
                
                amount
                epochId
                accountByAccountId {
                    alias
                    idSs58
                    role
                }
                eventByEventId {
                    blockByBlockId {
                        timestamp
                    }
                }
            }
        }
    }
}
```

## Swaps

```graphql
{
    allSwaps(orderBy: ID_ASC, first: NUMBER_OF_RESULTS, filter: {
        id: { greaterThan: LAST_ID }
        }) {
        edges {
            node {
                id
                nativeId
                type
                swapScheduledBlockTimestamp

                depositAmount
                depositValueUsd
                sourceAsset
                sourceChain

                egressAmount
                egressValueUsd
                destinationAsset
                destinationChain
                destinationAddress

                intermediateAmount
                intermediateValueUsd
                
                swapInputAmount
                swapInputValueUsd
                swapOutputAmount
                swapOutputValueUsd
                
                egress: eventByEgressScheduledEventId {
                    blockByBlockId {
                        timestamp
                    }
                }
                
                predeposit: foreignChainTrackingByForeignChainPreDepositBlockId {
                    stateChainTimestamp
                }
                
                deposit: foreignChainTrackingByForeignChainDepositBlockId {
                    stateChainTimestamp
                }
                
                swapChannelByDepositChannelId {
                    blockByIssuedBlockId {
                        timestamp
                    }
                    swapChannelBeneficiariesByDepositChannelId {
                        nodes {
                            brokerCommissionRateBps
                            type
                            brokerByBrokerId {
                                accountByAccountId {
                                    idSs58
                                }
                            }
                        }
                    }
                    brokerByBrokerId {
                        accountByAccountId {
                            idSs58
                        }
                    }
                }
                
                effectiveBoostFeeBps
                
                swapFeesBySwapId {
                    edges {
                        node {
                            type
                            amount
                            asset
                            valueUsd
                        }
                    }
                }
            }
        }
    }
}
```

## Weekly LP Overview

```graphql
{
    allAccounts {
        nodes {
            idSs58
            limitOrders: limitOrderFillsByLiquidityProviderId(
                filter: {
                    blockTimestamp: {
                        greaterThanOrEqualTo: \"TIME_FROM\", 
                        lessThan: \"TIME_TO\"
                    }
                }
            ) {
                groupedAggregates(groupBy: BLOCK_TIMESTAMP_TRUNCATED_TO_DAY) {
                    sum {
                        feesEarnedValueUsd
                        filledAmountValueUsd
                    }
                    keys
                }
            }
            rangeOrders: rangeOrderFillsByLiquidityProviderId(
                filter: {
                    blockTimestamp: {
                        greaterThanOrEqualTo: \"TIME_FROM\", 
                        lessThan: \"TIME_TO\"
                    }
                }
            ) {
                groupedAggregates(groupBy: BLOCK_TIMESTAMP_TRUNCATED_TO_DAY) {
                    sum {
                        quoteFeesEarnedValueUsd
                        quoteFilledAmountValueUsd
                        baseFeesEarnedValueUsd
                        baseFilledAmountValueUsd
                    }
                    keys
                }
            }
        }
    }
}
```